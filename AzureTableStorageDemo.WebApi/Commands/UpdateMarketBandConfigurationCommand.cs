using Azure;
using Azure.Data.Tables;
using AzureTableStorageDemo.WebApi.Helpers.AzureStorage.Entities;
using MediatR;

namespace AzureTableStorageDemo.WebApi.Commands
{
    public record UpdateMarketBandConfigurationCommand(MarketBandConfiguration MarketBandConfiguration) : IRequest;
}
