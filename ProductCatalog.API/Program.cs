using FluentValidation;
using Microsoft.AspNetCore.RateLimiting;
using ProductCatalog.API.Endpoints;
using ProductCatalog.API.Models;
using ProductCatalog.API.Repositories;
using ProductCatalog.API.Validators;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddScoped<IValidator<CreateProductDto>, CreateProductDtoValidator>();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("create-product", limiter =>
    {
        limiter.Window = TimeSpan.FromMinutes(1);
        limiter.PermitLimit = 10;
        limiter.QueueLimit = 0;
        limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
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
