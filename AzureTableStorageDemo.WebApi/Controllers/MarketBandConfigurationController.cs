using AzureTableStorageDemo.WebApi.Commands;
using AzureTableStorageDemo.WebApi.Helpers.AzureStorage.Entities;
using AzureTableStorageDemo.WebApi.Helpers.AzureStorage.Entities.Validators;
using AzureTableStorageDemo.WebApi.Helpers.Response;
using AzureTableStorageDemo.WebApi.Helpers.Responses;
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

        /// <summary>
        /// Updates the given <paramref name="marketBandConfiguration"/> object.
        /// </summary>
        /// <param name="marketBandConfiguration">The market band configuration object to be updated.</param>
        /// <returns>Returns a 201 Created response if the object was updated successfully, 
        /// a 400 Bad Request response if the request payload is invalid or 
        /// a 500 Internal Server Error response if an error occurred during processing.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] MarketBandConfiguration marketBandConfiguration)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the Update request.");

                var errorResponse=  new ErrorResponse
                {
                    Message = "An error occurred while processing the Update request."
                };

                return errorResponse;
            }
        }
    }
}
