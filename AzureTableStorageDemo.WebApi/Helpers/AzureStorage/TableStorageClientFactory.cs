using Azure.Data.Tables;
using AzureTableStorageDemo.WebApi.Helpers.Configurations;
using Microsoft.Extensions.Options;

namespace AzureTableStorageDemo.WebApi.Helpers.AzureStorage
{
    public class TableStorageClientFactory : ITableStorageClientFactory
    {
        private readonly AzureStorageConfigOption _azureStorageConfigOption;

        public TableStorageClientFactory(IOptions<AzureStorageConfigOption> options)
        {
            _azureStorageConfigOption = options.Value;
        }

        public TableServiceClient CreateTableServiceClient()
        {
            return new TableServiceClient(_azureStorageConfigOption.ConnectionString);
        }
    }
}
