using ProductCatalog.API.Exceptions;
using ProductCatalog.API.Models;
using ProductCatalog.API.Repositories;

namespace ProductCatalog.API.Services;

public class ProductService(IProductRepository repo, ILogger<ProductService> logger) : IProductService
{
    public Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default) =>
        repo.GetAllAsync(ct);

    public async Task<Product> CreateAsync(CreateProductDto dto, CancellationToken ct = default)
    {
        if (await repo.ExistsAsync(dto.Code, ct))
        {
            logger.LogWarning("Attempt to create product with duplicate code {Code}", dto.Code);
            throw new ProductCodeAlreadyExistsException(dto.Code);
        }

        var product = new Product
        {
            Code = dto.Code,
            Name = dto.Name,
            Price = dto.Price
        };

        var created = await repo.AddAsync(product, ct);
        logger.LogInformation("Product created with Id {Id} and Code {Code}", created.Id, created.Code);
        return created;
    }
}
