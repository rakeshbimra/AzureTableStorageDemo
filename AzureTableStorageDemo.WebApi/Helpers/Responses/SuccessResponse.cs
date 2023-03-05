﻿using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;

namespace AzureTableStorageDemo.WebApi.Helpers.Response
{
    public class SuccessResponse : ActionResult
    {
        public int Status { get; set; } = StatusCodes.Status200OK;
        public string Message { get; set; }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(new
            {
                Status,
                Message,
   
            })
            {
                StatusCode = Status,
                ContentTypes = new MediaTypeCollection { "application/json" }
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}
