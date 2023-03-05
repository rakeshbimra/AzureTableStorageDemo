﻿using AzureTableStorageDemo.WebApi.Commands;
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

            var expectedResult = new StatusCodeResult(201);

            _mockMediator
                .Setup(x => x.Send(It.IsAny<UpdateMarketBandConfigurationCommand>(), default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(marketBandConfiguration);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
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
            result.Should().BeOfType<BadRequestObjectResult>();
           
        }
    }
}