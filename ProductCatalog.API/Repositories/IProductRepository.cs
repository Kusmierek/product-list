using ProductCatalog.API.Models;

namespace ProductCatalog.API.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default);
    Task<bool> ExistsAsync(string code, CancellationToken ct = default);
    Task<Product> AddAsync(Product product, CancellationToken ct = default);
}
