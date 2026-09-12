using ProductCatalog.API.Models;

namespace ProductCatalog.API.Repositories;

public interface IProductRepository
{
    IEnumerable<Product> GetAll();
    Product Add(Product product);
}
