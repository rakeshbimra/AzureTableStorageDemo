using AzureTableStorageDemo.WebApi.Helpers.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTableStorageDemo.WebApi.Tests.Helpers.Extensions
{
    [TestClass]
    public class FluentValidationOptionsExtensionsTests
    {
        [TestMethod]
        public void AddWithValidation_ShouldAddValidatorAndOptions_WhenCalled()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(config);

            // Act
            services.AddWithValidation<TestOptions, TestOptionsValidator>("TestOptions");

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetService<IOptions<TestOptions>>();
            var validator = provider.GetService<IValidator<TestOptions>>();

            Assert.IsNotNull(options);
            Assert.IsNotNull(validator);
        }
    }

    public class TestOptions
    {
        public string Name { get; set; }
    }

    public class TestOptionsValidator : AbstractValidator<TestOptions>
    {
        public TestOptionsValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
