using ProductCatalog.API.Models;
using System.Collections.Concurrent;

namespace ProductCatalog.API.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new(
        new[]
        {
            new Product { Code = "P001", Name = "Laptop Pro 15",        Price = 4999.99m,  CreatedAt = DateTime.UtcNow.AddDays(-30) },
            new Product { Code = "P002", Name = "Wireless Mouse",        Price =   89.99m,  CreatedAt = DateTime.UtcNow.AddDays(-28) },
            new Product { Code = "P003", Name = "Mechanical Keyboard",   Price =  349.00m,  CreatedAt = DateTime.UtcNow.AddDays(-25) },
            new Product { Code = "P004", Name = "27\" 4K Monitor",       Price = 2199.00m,  CreatedAt = DateTime.UtcNow.AddDays(-22) },
            new Product { Code = "P005", Name = "USB-C Hub 7-in-1",      Price =  149.99m,  CreatedAt = DateTime.UtcNow.AddDays(-20) },
            new Product { Code = "P006", Name = "Noise-Cancelling Headphones", Price = 899.00m, CreatedAt = DateTime.UtcNow.AddDays(-18) },
            new Product { Code = "P007", Name = "Webcam HD 1080p",       Price =  229.00m,  CreatedAt = DateTime.UtcNow.AddDays(-15) },
            new Product { Code = "P008", Name = "Desk Lamp LED",         Price =   79.99m,  CreatedAt = DateTime.UtcNow.AddDays(-14) },
            new Product { Code = "P009", Name = "Ergonomic Chair",       Price = 1499.00m,  CreatedAt = DateTime.UtcNow.AddDays(-12) },
            new Product { Code = "P010", Name = "Standing Desk",         Price = 2599.00m,  CreatedAt = DateTime.UtcNow.AddDays(-11) },
            new Product { Code = "P011", Name = "External SSD 1TB",      Price =  349.99m,  CreatedAt = DateTime.UtcNow.AddDays(-10) },
            new Product { Code = "P012", Name = "Laptop Stand",          Price =  129.00m,  CreatedAt = DateTime.UtcNow.AddDays(-9)  },
            new Product { Code = "P013", Name = "Wireless Charger Pad",  Price =   59.99m,  CreatedAt = DateTime.UtcNow.AddDays(-8)  },
            new Product { Code = "P014", Name = "Mechanical Numpad",     Price =  179.00m,  CreatedAt = DateTime.UtcNow.AddDays(-7)  },
            new Product { Code = "P015", Name = "Cable Management Kit",  Price =   34.99m,  CreatedAt = DateTime.UtcNow.AddDays(-6)  },
            new Product { Code = "P016", Name = "Gaming Mouse Pad XL",   Price =   49.99m,  CreatedAt = DateTime.UtcNow.AddDays(-5)  },
            new Product { Code = "P017", Name = "Microphone USB",        Price =  399.00m,  CreatedAt = DateTime.UtcNow.AddDays(-4)  },
            new Product { Code = "P018", Name = "Thunderbolt 4 Dock",    Price =  799.00m,  CreatedAt = DateTime.UtcNow.AddDays(-3)  },
            new Product { Code = "P019", Name = "Portable Monitor 15\"", Price =  699.00m,  CreatedAt = DateTime.UtcNow.AddDays(-2)  },
            new Product { Code = "P020", Name = "Smart Power Strip",     Price =  119.99m,  CreatedAt = DateTime.UtcNow.AddDays(-1)  },
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
