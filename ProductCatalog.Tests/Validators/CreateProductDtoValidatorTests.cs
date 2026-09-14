using FluentAssertions;
using ProductCatalog.API.Models;
using ProductCatalog.API.Validators;

namespace ProductCatalog.Tests.Validators;

public class CreateProductDtoValidatorTests
{
    private readonly CreateProductDtoValidator _sut = new();

    [Fact]
    public async Task Validate_WhenDtoIsValid_PassesValidation()
    {
        var dto = new CreateProductDto { Code = "P001", Name = "Laptop", Price = 999.99m };

        var result = await _sut.ValidateAsync(dto);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Validate_WhenCodeIsEmpty_FailsValidation(string code)
    {
        var dto = new CreateProductDto { Code = code, Name = "Laptop", Price = 100m };

        var result = await _sut.ValidateAsync(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateProductDto.Code));
    }

    [Fact]
    public async Task Validate_WhenCodeExceedsMaxLength_FailsValidation()
    {
        var dto = new CreateProductDto { Code = "TOOLONG", Name = "Laptop", Price = 100m };

        var result = await _sut.ValidateAsync(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateProductDto.Code));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Validate_WhenNameIsEmpty_FailsValidation(string name)
    {
        var dto = new CreateProductDto { Code = "P001", Name = name, Price = 100m };

        var result = await _sut.ValidateAsync(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateProductDto.Name));
    }

    [Fact]
    public async Task Validate_WhenNameExceedsMaxLength_FailsValidation()
    {
        var dto = new CreateProductDto { Code = "P001", Name = new string('a', 101), Price = 100m };

        var result = await _sut.ValidateAsync(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateProductDto.Name));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999)]
    public async Task Validate_WhenPriceIsNotPositive_FailsValidation(decimal price)
    {
        var dto = new CreateProductDto { Code = "P001", Name = "Laptop", Price = price };

        var result = await _sut.ValidateAsync(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateProductDto.Price));
    }

    [Theory]
    [InlineData(1_000_000)]
    [InlineData(9_999_999)]
    public async Task Validate_WhenPriceExceedsLimit_FailsValidation(decimal price)
    {
        var dto = new CreateProductDto { Code = "P001", Name = "Laptop", Price = price };

        var result = await _sut.ValidateAsync(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateProductDto.Price));
    }
}
