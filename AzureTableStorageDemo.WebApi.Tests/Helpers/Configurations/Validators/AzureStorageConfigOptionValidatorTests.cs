using AzureTableStorageDemo.WebApi.Helpers.Configurations.Validators;
using AzureTableStorageDemo.WebApi.Helpers.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTableStorageDemo.WebApi.Tests.Helpers.Configurations.Validators
{
    [TestClass]
    public class AzureStorageConfigOptionValidatorTests
    {
        private AzureStorageConfigOptionValidator _validator;

        [TestInitialize]
        public void Initialize()
        {
            _validator = new AzureStorageConfigOptionValidator();
        }

        [TestMethod]
        public void Validate_ValidConfigOption_ShouldNotHaveValidationError()
        {
            // Arrange
            var configOption = new AzureStorageConfigOption
            {
                ConnectionString = "valid_connection_string"
            };

            // Act
            var result = _validator.Validate(configOption);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_EmptyConnectionString_ShouldHaveValidationError()
        {
            // Arrange
            var configOption = new AzureStorageConfigOption
            {
                ConnectionString = string.Empty
            };

            // Act
            var result = _validator.Validate(configOption);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Errors[0].PropertyName, "ConnectionString");
            Assert.AreEqual(result.Errors[0].ErrorMessage, "'Connection String' must not be empty.");
        }
    }

}
