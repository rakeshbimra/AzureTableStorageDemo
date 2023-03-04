using Azure;
using Azure.Data.Tables;

namespace AzureTableStorageDemo.WebApi.Helpers.AzureStorage.Entities
{
    public class MarketBandConfiguration : ITableEntity
    {
        public string PayerNumber { get; set; }
        public string MarketBandName { get; set; }
        public bool IsInvoiceAsPdfInEmail { get; set; }


        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string PartitionKey { get; set; }

        public MarketBandConfiguration()
        {
            PartitionKey = PayerNumber;
            RowKey = $"{PayerNumber}_{MarketBandName}";
        }
    }
}
