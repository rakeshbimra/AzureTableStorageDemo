using AzureTableStorageDemo.WebApi.Helpers.Configurations.Validators;
using Microsoft.Extensions.Options;

namespace AzureTableStorageDemo.WebApi.Helpers.Extensions
{
    public static class OptionsBuilderFluentValidationExtensions
    {
        /// <summary>
        /// Register this options instance for validation using FluentValidation
        /// Note that you _can't_ use async validators, or you will get an exception at runtime.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="optionsBuilder">The options builder to add the services to.</param>
        /// <returns>The <see cref="OptionsBuilder{TOptions}"/> so that additional calls can be chained.</returns>
        public static OptionsBuilder<TOptions> ValidateFluentValidation<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
        {
            optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
                provider => new FluentValidationOptions<TOptions>(optionsBuilder.Name, provider));
            return optionsBuilder;
        }
    }
}
