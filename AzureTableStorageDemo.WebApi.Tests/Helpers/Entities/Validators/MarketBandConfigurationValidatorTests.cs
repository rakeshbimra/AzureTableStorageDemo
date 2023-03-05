using AzureTableStorageDemo.WebApi.Helpers.AzureStorage.Entities.Validators;
using AzureTableStorageDemo.WebApi.Helpers.AzureStorage.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTableStorageDemo.WebApi.Tests.Helpers.Entities.Validators
{
    [TestClass]
    public class MarketBandConfigurationValidatorTests
    {
        private MarketBandConfigurationValidator _validator;

        [TestInitialize]
        public void Initialize()
        {
            _validator = new MarketBandConfigurationValidator();
        }

        [TestMethod]
        public void Validate_ValidMarketBandConfiguration_ReturnsTrue()
        {
            // Arrange
            var marketBandConfiguration = new MarketBandConfiguration
            {
                PayerNumber = "1234",
                MarketBandName = "Test Market Band"
            };

            // Act
            var result = _validator.Validate(marketBandConfiguration);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_InvalidMarketBandConfiguration_ReturnsFalse()
        {
            // Arrange
            var marketBandConfiguration = new MarketBandConfiguration
            {
                PayerNumber = "",
                MarketBandName = ""
            };

            // Act
            var result = _validator.Validate(marketBandConfiguration);

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_InvalidPayerNumber_ReturnsFalse()
        {
            // Arrange
            var marketBandConfiguration = new MarketBandConfiguration
            {
                PayerNumber = "",
                MarketBandName = "Test Market Band"
            };

            // Act
            var result = _validator.Validate(marketBandConfiguration);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Errors.Count, 1);
            Assert.AreEqual(result.Errors[0].ErrorMessage, "PayerNumber is required.");
        }

        [TestMethod]
        public void Validate_InvalidMarketBandName_ReturnsFalse()
        {
            // Arrange
            var marketBandConfiguration = new MarketBandConfiguration
            {
                PayerNumber = "1234",
                MarketBandName = ""
            };

            // Act
            var result = _validator.Validate(marketBandConfiguration);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Errors.Count, 1);
            Assert.AreEqual(result.Errors[0].ErrorMessage, "MarketBandName is required.");
        }
    }
}
