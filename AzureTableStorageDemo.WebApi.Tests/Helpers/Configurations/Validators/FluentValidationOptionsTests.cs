using AzureTableStorageDemo.WebApi.Helpers.Configurations.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTableStorageDemo.WebApi.Tests.Helpers.Configurations.Validators
{
    [TestClass]
    public class FluentValidationOptionsTests
    {
        [TestMethod]
        public void Validate_ValidOptions_ReturnsSuccess()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IValidator<TestOptions>, TestOptionsValidator>()
                .BuildServiceProvider();
            var sut = new FluentValidationOptions<TestOptions>(null, serviceProvider);
            var options = new TestOptions { Name = "Test" };

            // Act
            var result = sut.Validate(null, options);

            // Assert
            Assert.IsTrue(result.Succeeded);
        }

        [TestMethod]
        public void Validate_InvalidOptions_ReturnsFail()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IValidator<TestOptions>, TestOptionsValidator>()
                .BuildServiceProvider();
            var sut = new FluentValidationOptions<TestOptions>(null, serviceProvider);
            var options = new TestOptions { Name = null };

            // Act
            var result = sut.Validate(null, options);

            // Assert
            Assert.IsFalse(result.Succeeded);
        }

        private class TestOptions
        {
            public string? Name { get; set; }
        }

        private class TestOptionsValidator : AbstractValidator<TestOptions>
        {
            public TestOptionsValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
            }
        }
    }
}
