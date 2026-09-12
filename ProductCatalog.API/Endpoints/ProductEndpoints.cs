using ProductCatalog.API.Models;
using ProductCatalog.API.Repositories;

namespace ProductCatalog.API.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products");

        group.MapGet("/", (IProductRepository repo) => repo.GetAll());

        group.MapPost("/", (CreateProductDto dto, IProductRepository repo) =>
        {
            var product = new Product
            {
                Code = dto.Code,
                Name = dto.Name,
                Price = dto.Price
            };
            var created = repo.Add(product);
            return Results.Created($"/api/products/{created.Id}", created);
        });
    }
}
