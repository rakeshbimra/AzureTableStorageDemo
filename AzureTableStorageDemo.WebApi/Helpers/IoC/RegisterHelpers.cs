using AzureTableStorageDemo.WebApi.Helpers.AzureStorage;
using AzureTableStorageDemo.WebApi.Helpers.AzureStorage.Entities;
using AzureTableStorageDemo.WebApi.Helpers.Configurations;
using AzureTableStorageDemo.WebApi.Helpers.Configurations.Validators;
using AzureTableStorageDemo.WebApi.Helpers.Extensions;
using FluentAssertions.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace AzureTableStorageDemo.WebApi.Helpers.IoC
{
    public static class RegisterHelpers
    {
        public static void AddHelpers(this WebApplicationBuilder builder)
        {
           

            // Register TableStorage<T> as a singleton
            builder.Services.AddSingleton<ITableStorage<MarketBandConfiguration>, TableStorage<MarketBandConfiguration>>();
            builder.Services.AddSingleton<ITableStorageClientFactory, TableStorageClientFactory>();

            // Use the extension method
            builder.Services.AddWithValidation<AzureStorageConfigOption, AzureStorageConfigOptionValidator>("AzureStorageConfigOption");
        }
    }
}
