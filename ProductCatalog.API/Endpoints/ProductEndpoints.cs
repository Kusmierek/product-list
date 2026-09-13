using FluentValidation;
using ProductCatalog.API.Models;
using ProductCatalog.API.Repositories;

namespace ProductCatalog.API.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products");

        group.MapGet("/", async (IProductRepository repo, CancellationToken ct) =>
            await repo.GetAllAsync(ct));

        group.MapPost("/", async (IValidator<CreateProductDto> validator, CreateProductDto dto, IProductRepository repo, CancellationToken ct) =>
        {
            var validationResult = await validator.ValidateAsync(dto, ct);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());

            var product = new Product
            {
                Code = dto.Code,
                Name = dto.Name,
                Price = dto.Price
            };
            var created = await repo.AddAsync(product, ct);
            return Results.Created($"/api/products/{created.Id}", created);
        }).RequireRateLimiting("create-product");
    }
}
