using AzureTableStorageDemo.WebApi.Commands.Handlers;
using AzureTableStorageDemo.WebApi.Helpers.AzureStorage.Entities;
using AzureTableStorageDemo.WebApi.Helpers.AzureStorage;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureTableStorageDemo.WebApi.Commands;

namespace AzureTableStorageDemo.WebApi.Tests.Commands.Handlers
{
    [TestClass]
    public class UpdateMarketBandConfigurationCommandHandlerTests
    {
        private Mock<ITableStorage<MarketBandConfiguration>> _mockTableStorage;
        private Mock<ILogger<UpdateMarketBandConfigurationCommandHandler>> _mockLogger;
        private UpdateMarketBandConfigurationCommandHandler _handler;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockTableStorage = new Mock<ITableStorage<MarketBandConfiguration>>();
            _mockLogger = new Mock<ILogger<UpdateMarketBandConfigurationCommandHandler>>();
            _handler = new UpdateMarketBandConfigurationCommandHandler(_mockTableStorage.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldUpdateExistingItem_WhenAnItemWithTheSamePartitionKeyAndRowKeyExists()
        {
            // Arrange
            var marketBandConfiguration = new MarketBandConfiguration
            {
                PartitionKey = "TestPartitionKey",
                RowKey = "TestRowKey",
                IsInvoiceAsPdfInEmail = false
            };

            _mockTableStorage.Setup(x => x.GetAsync(marketBandConfiguration.PartitionKey, marketBandConfiguration.RowKey))
                .ReturnsAsync(new MarketBandConfiguration
                {
                    PartitionKey = marketBandConfiguration.PartitionKey,
                    RowKey = marketBandConfiguration.RowKey,
                    IsInvoiceAsPdfInEmail = true
                });

            // Act
            await _handler.Handle(new UpdateMarketBandConfigurationCommand(marketBandConfiguration), CancellationToken.None);

            // Assert
            _mockTableStorage.Verify(x => x.UpdateAsync(It.Is<MarketBandConfiguration>(y => y.PartitionKey == marketBandConfiguration.PartitionKey && y.RowKey == marketBandConfiguration.RowKey && y.IsInvoiceAsPdfInEmail == marketBandConfiguration.IsInvoiceAsPdfInEmail)), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ShouldAddNewItem_WhenAnItemWithTheSamePartitionKeyAndRowKeyDoesNotExist()
        {
            // Arrange
            var marketBandConfiguration = new MarketBandConfiguration
            {
                PartitionKey = "TestPartitionKey",
                RowKey = "TestRowKey",
                IsInvoiceAsPdfInEmail = true
            };

            _mockTableStorage.Setup(x => x.GetAsync(marketBandConfiguration.PartitionKey, marketBandConfiguration.RowKey))
                .ReturnsAsync((MarketBandConfiguration)null);

            // Act
            await _handler.Handle(new UpdateMarketBandConfigurationCommand(marketBandConfiguration), CancellationToken.None);

            // Assert
            _mockTableStorage.Verify(x => x.AddAsync(It.Is<MarketBandConfiguration>(y => y.PartitionKey == marketBandConfiguration.PartitionKey && y.RowKey == marketBandConfiguration.RowKey && y.IsInvoiceAsPdfInEmail == marketBandConfiguration.IsInvoiceAsPdfInEmail)), Times.Once);
        }
    }
}
