using AzureTableStorageDemo.WebApi.Helpers.AzureStorage.Entities;
using AzureTableStorageDemo.WebApi.Helpers.AzureStorage;
using AzureTableStorageDemo.WebApi.Helpers.Configurations;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureTableStorageDemo.WebApi.Helpers.IoC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AzureTableStorageDemo.WebApi.Tests.Helpers.IoC
{
    [TestClass]
    public class RegisterHelpersTests
    {
        [TestMethod]
        public void AddHelpers_ShouldRegisterServices()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.AddHelpers();

            // Assert
            var serviceProvider = builder.Services.BuildServiceProvider();

            var tableStorage = serviceProvider.GetService<ITableStorage<MarketBandConfiguration>>();
            Assert.IsNotNull(tableStorage);

            var tableStorageClientFactory = serviceProvider.GetService<ITableStorageClientFactory>();
            Assert.IsNotNull(tableStorageClientFactory);

            var validationOptions = serviceProvider.GetService<IOptions<AzureStorageConfigOption>>();
            Assert.IsNotNull(validationOptions);

            var validationOptionsValidator = serviceProvider.GetService<IValidator<AzureStorageConfigOption>>();
            Assert.IsNotNull(validationOptionsValidator);
        }
    }

}
