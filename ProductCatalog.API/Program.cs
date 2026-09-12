using ProductCatalog.API.Endpoints;
using ProductCatalog.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();

var app = builder.Build();

app.MapProductEndpoints();

app.Run();
