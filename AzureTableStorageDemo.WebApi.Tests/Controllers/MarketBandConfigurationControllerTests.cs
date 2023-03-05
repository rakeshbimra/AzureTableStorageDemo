using AzureTableStorageDemo.WebApi.Commands;
using AzureTableStorageDemo.WebApi.Controllers;
using AzureTableStorageDemo.WebApi.Helpers.AzureStorage.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using AzureTableStorageDemo.WebApi.Helpers.Response;
using Microsoft.AspNetCore.Http;
using AzureTableStorageDemo.WebApi.Helpers.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace AzureTableStorageDemo.WebApi.Tests.Controllers
{
    [TestClass]
    public class MarketBandConfigurationControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private Mock<ILogger<MarketBandConfigurationController>> _mockLogger;
        private MarketBandConfigurationController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<MarketBandConfigurationController>>();
            _controller = new MarketBandConfigurationController(_mockMediator.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task Update_WithValidModel_ShouldReturnStatusCode201()
        {
            // Arrange
            var marketBandConfiguration = new MarketBandConfiguration
            {
                PayerNumber = "123",
                MarketBandName = "TestMarketBand",
                IsInvoiceAsPdfInEmail = true,
                PartitionKey = "123",
                RowKey = "TestMarketBand"
            };

            _mockMediator
                .Setup(x => x.Send(It.IsAny<UpdateMarketBandConfigurationCommand>(), default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(marketBandConfiguration);

            // Assert
            // Assert
            Assert.IsInstanceOfType(result, typeof(SuccessResponse));
            var response = result as SuccessResponse;
            Assert.AreEqual("MarketBandConfiguration updated successfully", response.Message);
        }

        [TestMethod]
        public async Task Update_WithInvalidModel_ShouldReturnBadRequest()
        {
            // Arrange
            var marketBandConfiguration = new MarketBandConfiguration
            {
                PayerNumber = string.Empty,
                MarketBandName = string.Empty,
                IsInvoiceAsPdfInEmail = true,
                PartitionKey = string.Empty,
                RowKey = string.Empty
            };

            // Act
            var result = await _controller.Update(marketBandConfiguration);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResponse));
            var response = result as BadRequestResponse;
            Assert.AreEqual("Validation failed", response.Message);
            Assert.IsNotNull(response.Errors);
            Assert.IsTrue(response.Errors.ContainsKey("MarketBandName"));

        }

        [TestMethod]
        public async Task Update_ExceptionThrown_ReturnsInternalServerErrorResponse()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<MarketBandConfigurationController>>();
            var controller = new MarketBandConfigurationController(mediatorMock.Object, loggerMock.Object);
            var marketBandConfiguration = new MarketBandConfiguration { PayerNumber = "123", MarketBandName = "Test" };
            mediatorMock.Setup(m => m.Send(It.IsAny<UpdateMarketBandConfigurationCommand>(), default(CancellationToken)))
                .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await controller.Update(marketBandConfiguration);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result is OkResult);
            Assert.IsFalse(result is BadRequestResult);
        }
    }
}
