using eCommerce.ProductsService.Application.DTOs;
using FluentValidation;

namespace eCommerce.ProductsService.Application.Validators;

public class AddProductDtoValidator : AbstractValidator<AddProductDto>
{
    public AddProductDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(p => p.Category)
            .IsInEnum();

        RuleFor(p => p.UnitPrice)
            .InclusiveBetween(1, 99999);

        RuleFor(p => p.QuantityInStock)
            .InclusiveBetween(0, 999);
    }
}
