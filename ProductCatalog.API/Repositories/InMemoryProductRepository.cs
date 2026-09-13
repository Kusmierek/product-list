using ProductCatalog.API.Models;
using System.Collections.Concurrent;

namespace ProductCatalog.API.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new(
        new[]
        {
            new Product { Code = "P001", Name = "Laptop Pro 15", Price = 4999.99m, CreatedAt = DateTime.UtcNow.AddDays(-10) },
            new Product { Code = "P002", Name = "Wireless Mouse", Price = 89.99m, CreatedAt = DateTime.UtcNow.AddDays(-5) },
            new Product { Code = "P003", Name = "Mechanical Keyboard", Price = 349.00m, CreatedAt = DateTime.UtcNow.AddDays(-1) }
        }.ToDictionary(p => p.Id));

    public Task<PagedResult<Product>> GetAllAsync(ProductQuery query, CancellationToken ct = default)
    {
        var items = _products.Values.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            items = items.Where(p =>
                p.Code.ToLower().Contains(search) ||
                p.Name.ToLower().Contains(search));
        }

        items = (query.SortBy, query.SortDir) switch
        {
            (ProductSortBy.Name,      SortDirection.Asc)  => items.OrderBy(p => p.Name),
            (ProductSortBy.Name,      SortDirection.Desc) => items.OrderByDescending(p => p.Name),
            (ProductSortBy.Price,     SortDirection.Asc)  => items.OrderBy(p => p.Price),
            (ProductSortBy.Price,     SortDirection.Desc) => items.OrderByDescending(p => p.Price),
            (ProductSortBy.CreatedAt, SortDirection.Asc)  => items.OrderBy(p => p.CreatedAt),
            _                                             => items.OrderByDescending(p => p.CreatedAt)
        };

        var total = items.Count();
        var paged = items.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize);

        return Task.FromResult(new PagedResult<Product>
        {
            Items = paged,
            Total = total,
            Page = query.Page,
            PageSize = query.PageSize
        });
    }

    public Task<bool> ExistsAsync(string code, CancellationToken ct = default) =>
        Task.FromResult(_products.Values.Any(p => p.Code == code));

    public Task<Product> AddAsync(Product product, CancellationToken ct = default)
    {
        _products[product.Id] = product;
        return Task.FromResult(product);
    }
}
