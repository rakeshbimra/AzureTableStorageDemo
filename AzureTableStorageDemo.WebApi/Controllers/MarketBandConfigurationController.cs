using AzureTableStorageDemo.WebApi.Commands;
using AzureTableStorageDemo.WebApi.Helpers.AzureStorage.Entities;
using AzureTableStorageDemo.WebApi.Helpers.AzureStorage.Entities.Validators;
using AzureTableStorageDemo.WebApi.Helpers.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AzureTableStorageDemo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketBandConfigurationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MarketBandConfigurationController> _logger;

        public MarketBandConfigurationController(IMediator mediator, ILogger<MarketBandConfigurationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update([FromBody] MarketBandConfiguration marketBandConfiguration)
        {
            _logger.LogInformation("{class} -> {method} -> Start", nameof(MarketBandConfigurationController), nameof(MarketBandConfigurationController.Update));

            _logger.LogInformation("Processing Update request. PayerNumber: {payerNumber}, MarketBandName: {marketBandName}",
                marketBandConfiguration.PayerNumber,
                marketBandConfiguration.MarketBandName);

            var validator = new MarketBandConfigurationValidator();

            var validationResult = validator.Validate(marketBandConfiguration);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for Update request. Errors: {errors}", validationResult.Errors);

                var response = new BadRequestResponse
                {
                    Message = "Validation failed",
                    Errors = validationResult.Errors
                        .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                        .ToDictionary(x => x.Key, x => x.AsEnumerable())
                };

                _logger.LogInformation("{class} -> {method} -> End", nameof(MarketBandConfigurationController), nameof(MarketBandConfigurationController.Update));

                return response;
            }

            await _mediator.Send(new UpdateMarketBandConfigurationCommand(marketBandConfiguration));

            _logger.LogInformation("Update request processed successfully. PayerNumber: {payerNumber}, MarketBandName: {marketBandName}",
                marketBandConfiguration.PayerNumber,
                marketBandConfiguration.MarketBandName);

            var successResponse = new SuccessResponse
            {
                Message = "MarketBandConfiguration updated successfully",
            };

            _logger.LogInformation("{class} -> {method} -> End", nameof(MarketBandConfigurationController), nameof(MarketBandConfigurationController.Update));

            return successResponse;
        }
    }
}
