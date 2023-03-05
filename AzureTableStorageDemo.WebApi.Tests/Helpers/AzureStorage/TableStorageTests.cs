using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using AzureTableStorageDemo.WebApi.Helpers.AzureStorage;
using AzureTableStorageDemo.WebApi.Tests.Helpers.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AzureTableStorageDemo.WebApi.Tests.Helpers.AzureStorage
{
    [TestClass]
    public class TableStorageTests
    {
        private TableStorage<TestEntity> _tableStorage;
        private Mock<ITableStorageClientFactory> _tableStorageClientFactoryMock;
        private Mock<TableClient> _tableClientMock;
        private Mock<ILogger<TableStorage<TestEntity>>> _loggerMock;

        [TestInitialize]
        public void Setup()
        {
            _tableStorageClientFactoryMock = new Mock<ITableStorageClientFactory>();
            _tableClientMock = new Mock<TableClient>(new Uri("http://localhost:10002/devstoreaccount1"), "table");
            _loggerMock = new Mock<ILogger<TableStorage<TestEntity>>>();

            _tableStorageClientFactoryMock.Setup(x => x.CreateTableServiceClient().GetTableClient(It.IsAny<string>()))
                .Returns(_tableClientMock.Object);

            _tableStorage = new TableStorage<TestEntity>(_loggerMock.Object, _tableStorageClientFactoryMock.Object);
        }
    }
}