using FluentValidation;
using ProductCatalog.API.Models;
using ProductCatalog.API.Repositories;

namespace ProductCatalog.API.Validators;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator(IProductRepository repo)
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => !repo.GetAll().Any(p => p.Code == code))
            .WithMessage("Product with this code already exists.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .LessThan(1_000_000);
    }
}
