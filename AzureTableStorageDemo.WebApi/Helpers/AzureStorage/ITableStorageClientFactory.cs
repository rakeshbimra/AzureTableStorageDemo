using Azure.Data.Tables;

namespace AzureTableStorageDemo.WebApi.Helpers.AzureStorage
{
    public interface ITableStorageClientFactory
    {
        TableServiceClient CreateTableServiceClient();
    }
}
