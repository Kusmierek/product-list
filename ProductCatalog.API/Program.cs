using FluentValidation;
using Microsoft.AspNetCore.RateLimiting;
using ProductCatalog.API.Endpoints;
using ProductCatalog.API.Models;
using ProductCatalog.API.Repositories;
using ProductCatalog.API.Services;
using ProductCatalog.API.Validators;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IValidator<CreateProductDto>, CreateProductDtoValidator>();

var rateLimitConfig = builder.Configuration.GetSection("RateLimiting:CreateProduct");
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("create-product", limiter =>
    {
        limiter.Window = TimeSpan.FromMinutes(rateLimitConfig.GetValue<int>("WindowMinutes"));
        limiter.PermitLimit = rateLimitConfig.GetValue<int>("PermitLimit");
        limiter.QueueLimit = 0;
        limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseRateLimiter();
app.UseCors("AllowAngular");
app.MapProductEndpoints();

app.Run();
