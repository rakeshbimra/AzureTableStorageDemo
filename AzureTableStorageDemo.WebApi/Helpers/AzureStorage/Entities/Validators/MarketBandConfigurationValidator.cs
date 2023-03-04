using FluentValidation;

namespace AzureTableStorageDemo.WebApi.Helpers.AzureStorage.Entities.Validators
{
    public class MarketBandConfigurationValidator:AbstractValidator<MarketBandConfiguration>
    {
        public MarketBandConfigurationValidator()
        {
            RuleFor(x => x.PayerNumber)
                .NotEmpty()
                .WithMessage("PayerNumber is required.");

            RuleFor(x => x.MarketBandName)
                .NotEmpty()
                .WithMessage("MarketBandName is required.");

            // Add additional rules for validating other properties as needed
        }
    }
}
