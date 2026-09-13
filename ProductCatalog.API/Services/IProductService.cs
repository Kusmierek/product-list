using ProductCatalog.API.Models;

namespace ProductCatalog.API.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default);
    Task<Product> CreateAsync(CreateProductDto dto, CancellationToken ct = default);
}
