using eCommerce.ProductsService.Application.Errors;
using FluentValidation.Results;

namespace eCommerce.ProductsService.Application.Exntensions;

public static class ValidationResultExtensions
{
    public static IEnumerable<ValidationError> ToValidationErrors(this ValidationResult validationResult)
    {
        return validationResult.Errors
            .Select(e => new ValidationError(e.ErrorMessage));
    }
}
