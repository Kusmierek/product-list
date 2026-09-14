using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ProductCatalog.API.Exceptions;
using ProductCatalog.API.Models;
using ProductCatalog.API.Repositories;
using ProductCatalog.API.Services;

namespace ProductCatalog.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly Mock<ILogger<ProductService>> _loggerMock = new();
    private readonly ProductService _sut;

    public ProductServiceTests()
    {
        _sut = new ProductService(_repoMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WhenCodeIsUnique_ReturnsCreatedProduct()
    {
        var dto = new CreateProductDto { Code = "P099", Name = "Test Product", Price = 99.99m };
        var expected = new Product { Code = dto.Code, Name = dto.Name, Price = dto.Price };

        _repoMock.Setup(r => r.ExistsAsync(dto.Code, default)).ReturnsAsync(false);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Product>(), default)).ReturnsAsync(expected);

        var result = await _sut.CreateAsync(dto);

        result.Should().BeEquivalentTo(expected);
        _repoMock.Verify(r => r.AddAsync(It.Is<Product>(p => p.Code == dto.Code && p.Name == dto.Name && p.Price == dto.Price), default), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenCodeAlreadyExists_ThrowsProductCodeAlreadyExistsException()
    {
        var dto = new CreateProductDto { Code = "P001", Name = "Duplicate", Price = 1m };

        _repoMock.Setup(r => r.ExistsAsync(dto.Code, default)).ReturnsAsync(true);

        await _sut.Awaiting(s => s.CreateAsync(dto))
            .Should().ThrowAsync<ProductCodeAlreadyExistsException>()
            .WithMessage($"*{dto.Code}*");

        _repoMock.Verify(r => r.AddAsync(It.IsAny<Product>(), default), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_DelegatesToRepository()
    {
        var query = new ProductQuery();
        var expected = new PagedResult<Product> { Items = [], Total = 0, Page = 1, PageSize = 10 };

        _repoMock.Setup(r => r.GetAllAsync(query, default)).ReturnsAsync(expected);

        var result = await _sut.GetAllAsync(query);

        result.Should().BeSameAs(expected);
    }
}
