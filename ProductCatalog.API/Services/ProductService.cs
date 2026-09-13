using ProductCatalog.API.Exceptions;
using ProductCatalog.API.Models;
using ProductCatalog.API.Repositories;

namespace ProductCatalog.API.Services;

public class ProductService(IProductRepository repo) : IProductService
{
    public Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default) =>
        repo.GetAllAsync(ct);

    public async Task<Product> CreateAsync(CreateProductDto dto, CancellationToken ct = default)
    {
        var existing = await repo.GetAllAsync(ct);
        if (existing.Any(p => p.Code == dto.Code))
            throw new ProductCodeAlreadyExistsException(dto.Code);

        var product = new Product
        {
            Code = dto.Code,
            Name = dto.Name,
            Price = dto.Price
        };

        return await repo.AddAsync(product, ct);
    }
}
