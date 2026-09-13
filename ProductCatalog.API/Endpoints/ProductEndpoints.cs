using FluentValidation;
using ProductCatalog.API.Exceptions;
using ProductCatalog.API.Models;
using ProductCatalog.API.Services;

namespace ProductCatalog.API.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products");

        group.MapGet("/", async (
            IProductService service,
            CancellationToken ct,
            int page = 1,
            int pageSize = 10,
            ProductSortBy sortBy = ProductSortBy.CreatedAt,
            SortDirection sortDir = SortDirection.Desc,
            string? search = null) =>
        {
            var query = new ProductQuery { Page = page, PageSize = pageSize, SortBy = sortBy, SortDir = sortDir, Search = search };
            return await service.GetAllAsync(query, ct);
        });

        group.MapPost("/", async (
            IValidator<CreateProductDto> validator,
            CreateProductDto dto,
            IProductService service,
            CancellationToken ct) =>
        {
            var validationResult = await validator.ValidateAsync(dto, ct);

            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());

            try
            {
                var created = await service.CreateAsync(dto, ct);
                return Results.Created($"/api/products/{created.Id}", created);
            }
            catch (ProductCodeAlreadyExistsException ex)
            {
                return Results.Conflict(new { error = ex.Message });
            }
        }).RequireRateLimiting("create-product");
    }
}
