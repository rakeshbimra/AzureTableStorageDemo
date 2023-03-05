using AzureTableStorageDemo.WebApi.Helpers.Configurations.Validators;
using AzureTableStorageDemo.WebApi.Helpers.Extensions;
using Microsoft.Extensions.DependencyInjection;
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
    public class OptionsBuilderFluentValidationExtensionsTests
    {
        [TestMethod]
        public void ValidateFluentValidation_ShouldRegisterValidator_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();
            var optionsBuilder = new OptionsBuilder<TestOptions>(services, "myOptions");

            // Act
            optionsBuilder.ValidateFluentValidation();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var validateOptions = serviceProvider.GetService<IValidateOptions<TestOptions>>();
            Assert.IsNotNull(validateOptions);
            Assert.IsInstanceOfType(validateOptions, typeof(FluentValidationOptions<TestOptions>));
        }
    }
}
