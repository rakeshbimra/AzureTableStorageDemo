using FluentValidation;
using Microsoft.Extensions.Options;

namespace AzureTableStorageDemo.WebApi.Helpers.Extensions
{
    public static class FluentValidationOptionsExtensions
    {
        public static OptionsBuilder<TOptions> AddWithValidation<TOptions, TValidator>(
            this IServiceCollection services,
            string configurationSection)
        where TOptions : class
        where TValidator : class, IValidator<TOptions>
        {
            services.AddScoped<IValidator<TOptions>, TValidator>();

            return services.AddOptions<TOptions>()
                .BindConfiguration(configurationSection)
                .ValidateFluentValidation()
                .ValidateOnStart();
        }
    }
}
