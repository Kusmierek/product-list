using ProductCatalog.API.Models;

namespace ProductCatalog.API.Services;

public interface IProductService
{
    Task<PagedResult<Product>> GetAllAsync(ProductQuery query, CancellationToken ct = default);
    Task<Product> CreateAsync(CreateProductDto dto, CancellationToken ct = default);
}
