using ProductCatalog.API.Models;

namespace ProductCatalog.API.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = new()
    {
        new Product { Code = "P001", Name = "Laptop Pro 15", Price = 4999.99m },
        new Product { Code = "P002", Name = "Wireless Mouse", Price = 89.99m },
        new Product { Code = "P003", Name = "Mechanical Keyboard", Price = 349.00m }
    };

    public Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default) =>
        Task.FromResult<IEnumerable<Product>>(_products.AsReadOnly());

    public Task<Product> AddAsync(Product product, CancellationToken ct = default)
    {
        _products.Add(product);
        return Task.FromResult(product);
    }
}
