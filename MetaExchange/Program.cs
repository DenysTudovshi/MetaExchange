using MetaExchange.Services;
using MetaExchange.Validators;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddValidatorsFromAssemblyContaining<ExecutionRequestValidator>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Meta-Exchange API",
        Version = "v1",
        Description = "API for finding best execution across multiple crypto exchanges"
    });
});

builder.Services.AddScoped<IOrderBookService, OrderBookService>();
builder.Services.AddScoped<IBestExecutionService, BestExecutionService>();

if (builder.Environment.IsDevelopment())
{
    // Use default configuration (HTTP + HTTPS with dev certs)
}
else
{
    // For Docker/Production: HTTP only
    builder.WebHost.UseUrls("http://0.0.0.0:8080");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
// Enable Swagger in all environments (not just Development)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meta-Exchange API V1");
    c.RoutePrefix = "swagger"; 
});

// Only use HTTPS redirection in Development with HTTPS
if (app.Environment.IsDevelopment())
{
    
}

app.UseAuthorization();
app.MapControllers();
app.Run();