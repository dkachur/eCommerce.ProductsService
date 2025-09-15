using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.API.Exntensions;
using eCommerce.ProductsService.API.Helpers;
using eCommerce.ProductsService.Application.Enums;
using FluentValidation;

namespace eCommerce.ProductsService.API.Validators
{
    public class UpdateProductRequestValidator: AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty();

            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(p => p.Category)
                .NotEmpty()
                .IsEnumName(typeof(CategoryOptions), false)
                .WithMessage("Category must be " + EnumHelper.GetOptionsString<CategoryOptions>() + ".");

            RuleFor(p => p.UnitPrice)
                .InclusiveBetween(1, 99999);

            RuleFor(p => p.QuantityInStock)
                .InclusiveBetween(0, 999);
        }
    }
}
