using FluentValidation;
using Microsoft.OpenApi.Models;
using Time.Off.Domain.Repositories;
using Time.Off.Application.UseCases.SubmitLeaveRequest;
using Time.Off.Infrastructure.Repositories;
using Time.Off.Infrastructure;
using Swashbuckle.AspNetCore.Filters;
using Time.Off.Api.SwaggerExamples;
using Microsoft.AspNetCore.Mvc;
using Time.Off.Application.UseCases.GetLeaveRequest;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Time Off API",
        Version = "v1",
        Description = "API for submitting leave requests",
    });

    c.EnableAnnotations();
    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<RequestLeaveCommandExample>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddDatabaseConfiguration();
builder.Services.AddScoped<IValidator<RequestLeaveCommand>, SubmitLeaveRequestValidator>();
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
builder.Services.AddScoped<SubmitLeaveRequestHandler>();
builder.Services.AddScoped<GetLeaveRequestByIdHandler>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Time Off API");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
