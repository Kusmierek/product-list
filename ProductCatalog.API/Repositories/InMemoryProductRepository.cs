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

    public IEnumerable<Product> GetAll() => _products.AsReadOnly();

    public Product Add(Product product)
    {
        _products.Add(product);
        return product;
    }
}
