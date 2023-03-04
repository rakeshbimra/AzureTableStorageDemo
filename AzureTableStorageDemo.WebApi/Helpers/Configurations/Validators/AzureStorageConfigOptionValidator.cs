using FluentValidation;

namespace AzureTableStorageDemo.WebApi.Helpers.Configurations.Validators
{
    public class AzureStorageConfigOptionValidator : AbstractValidator<AzureStorageConfigOption>
    {
        public AzureStorageConfigOptionValidator()
        {
            RuleFor(x => x.ConnectionString).NotEmpty();
        }
    }
}
