using FluentValidation;
using Microsoft.OpenApi.Models;
using Time.Off.Domain.Repositories;
using Time.Off.Application.UseCases.SubmitLeaveRequest;
using Time.Off.Infrastructure.Repositories;
using Time.Off.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo
{
    Title = "Time Off API",
    Version = "v1",
    Description = "API for submitting leave requests",
}));

builder.Services.AddDatabaseConfiguration();
builder.Services.AddScoped<IValidator<RequestLeaveCommand>, SubmitLeaveRequestValidator>();
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
builder.Services.AddScoped<SubmitLeaveRequestHandler>();
builder.Services.AddControllers();
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
