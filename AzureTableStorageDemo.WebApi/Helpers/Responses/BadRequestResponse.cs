using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;

namespace AzureTableStorageDemo.WebApi.Helpers.Response
{
    public class BadRequestResponse : ActionResult
    {
        public int Status { get; set; } = StatusCodes.Status400BadRequest;
        public string Message { get; set; }
        public IDictionary<string, IEnumerable<string>> Errors { get; set; }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(new
            {
                Status,
                Message,
                Errors
            })
            {
                StatusCode = Status,
                ContentTypes = new MediaTypeCollection { "application/json" }
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}
