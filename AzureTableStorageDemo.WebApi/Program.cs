using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Swashbuckle;
using AzureTableStorageDemo.WebApi.Commands.IoC;
using AzureTableStorageDemo.WebApi.Helpers.IoC;
using AzureTableStorageDemo.WebApi.Helpers.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(Program));

builder.AddHelpers();
builder.Services.AddCommands();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
