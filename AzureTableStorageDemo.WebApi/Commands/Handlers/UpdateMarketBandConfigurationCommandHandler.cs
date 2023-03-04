using AzureTableStorageDemo.WebApi.Helpers.AzureStorage;
using AzureTableStorageDemo.WebApi.Helpers.AzureStorage.Entities;
using MediatR;

namespace AzureTableStorageDemo.WebApi.Commands.Handlers
{
    public class UpdateMarketBandConfigurationCommandHandler : IRequestHandler<UpdateMarketBandConfigurationCommand, Unit>
    {
        private readonly ILogger<UpdateMarketBandConfigurationCommandHandler> _logger;
        private readonly ITableStorage<MarketBandConfiguration> _tableStorage;

        public UpdateMarketBandConfigurationCommandHandler(ITableStorage<MarketBandConfiguration> tableStorage, ILogger<UpdateMarketBandConfigurationCommandHandler> logger)
        {
            _tableStorage = tableStorage;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateMarketBandConfigurationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{class} -> {method}-> Start", nameof(UpdateMarketBandConfigurationCommandHandler), nameof(UpdateMarketBandConfigurationCommandHandler.Handle));

            try
            {
                _logger.LogInformation($"Updating market band configuration with partition key {request.MarketBandConfiguration.PartitionKey} and row key {request.MarketBandConfiguration.RowKey}");

                await _tableStorage.CreateTableIfNotExistsAsync();

                // Check if an item with the same partition key and row key exists in the table
                var existingItem = await _tableStorage.GetAsync(request.MarketBandConfiguration.PartitionKey, request.MarketBandConfiguration.RowKey);

                if (existingItem != null)
                {
                    // If an item already exists, update it
                    existingItem.IsInvoiceAsPdfInEmail = request.MarketBandConfiguration.IsInvoiceAsPdfInEmail;

                    await _tableStorage.UpdateAsync(existingItem);

                    _logger.LogInformation($"Market band configuration with partition key {request.MarketBandConfiguration.PartitionKey} and row key {request.MarketBandConfiguration.RowKey} updated successfully");
                }
                else
                {
                    await _tableStorage.AddAsync(request.MarketBandConfiguration);

                    _logger.LogInformation($"Market band configuration with partition key {request.MarketBandConfiguration.PartitionKey} and row key {request.MarketBandConfiguration.RowKey} added successfully");
                }

                _logger.LogInformation("{class} -> {method}-> End", nameof(UpdateMarketBandConfigurationCommandHandler), nameof(UpdateMarketBandConfigurationCommandHandler.Handle));

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating market band configuration with partition key {request.MarketBandConfiguration.PartitionKey} and row key {request.MarketBandConfiguration.RowKey}");
                throw;
            }
        }
    }
}
