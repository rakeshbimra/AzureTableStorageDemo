using AzureTableStorageDemo.WebApi.Helpers.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace AzureTableStorageDemo.WebApi.Tests.Helpers.Responses
{
    [TestClass]
    public class SuccessResponseTests
    {
        [TestMethod]
        public async Task ExecuteResultAsync_ReturnsSuccessResponse()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddMvc();
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

            var sut = new SuccessResponse
            {
                Message = "Success"
            };

            // Act
            var objectResult = new ObjectResult(new
            {
                sut.Status,
                sut.Message
            })
            {
                StatusCode = sut.Status,
                ContentTypes = new MediaTypeCollection { "application/json" }
            };

            await objectResult.ExecuteResultAsync(context);

            // Assert
            Assert.AreEqual(sut.Status, objectResult.StatusCode);
            var responseObject = (dynamic)objectResult.Value;
            Assert.AreEqual(sut.Message, responseObject.Message);
            Assert.AreEqual("application/json; charset=utf-8", context.HttpContext.Response.ContentType);
        }
    }

}
