using Azure.Data.Tables;
using AzureTableStorageDemo.WebApi.Helpers.AzureStorage;
using AzureTableStorageDemo.WebApi.Helpers.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTableStorageDemo.WebApi.Tests.Helpers.AzureStorage
{
    [TestClass]
    public class TableStorageClientFactoryTests
    {
        private IOptions<AzureStorageConfigOption> _options;

        [TestInitialize]
        public void SetUp()
        {
            // set up the configuration options
            _options = Options.Create(new AzureStorageConfigOption
            {
                ConnectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;"
            });
        }

        [TestMethod]
        public void CreateTableServiceClient_ShouldReturnTableServiceClientInstance()
        {
            // arrange
            var factory = new TableStorageClientFactory(_options);

            // act
            var result = factory.CreateTableServiceClient();

            // assert
            Assert.IsInstanceOfType(result, typeof(TableServiceClient));
        }

        
    }
}
