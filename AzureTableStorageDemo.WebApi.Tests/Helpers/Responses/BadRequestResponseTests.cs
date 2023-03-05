using AzureTableStorageDemo.WebApi.Helpers.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace AzureTableStorageDemo.WebApi.Tests.Helpers.Responses
{
    [TestClass]
    public class BadRequestResponseTests
    {
        [TestMethod]
        public async Task ExecuteResultAsync_ReturnsBadRequestResponse()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddMvc();
            services.AddLogging();
            var serviceProvider = services.BuildServiceProvider();

            var context = new ActionContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = serviceProvider
                },
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor(),
            };

            var sut = new BadRequestResponse
            {
                Message = "Bad Request",
                Errors = new Dictionary<string, IEnumerable<string>>
        {
            { "PayerNumber", new[] { "PayerNumber is required" } },
            { "PartitionKey", new[] { "PartitionKey is required" } }
        }
            };

            // Act
            var objectResult = new ObjectResult(new
            {
                sut.Status,
                sut.Message,
                sut.Errors
            })
            {
                StatusCode = sut.Status,
                ContentTypes = new MediaTypeCollection { "application/json" }
            };

            await objectResult.ExecuteResultAsync(context);

            // Assert
            var responseObject = (dynamic)objectResult.Value;
            Assert.AreEqual(sut.Status, responseObject.Status);
            Assert.AreEqual(sut.Message, responseObject.Message);
            Assert.IsInstanceOfType(responseObject.Errors, typeof(IDictionary<string, IEnumerable<string>>));
            var errors = (IDictionary<string, IEnumerable<string>>)responseObject.Errors;
            Assert.AreEqual(sut.Errors.Count, errors.Count);
            foreach (var kvp in sut.Errors)
            {
                Assert.IsTrue(errors.ContainsKey(kvp.Key));
                CollectionAssert.AreEqual(kvp.Value.ToList(), errors[kvp.Key].ToList());
            }
        }
    }
}
