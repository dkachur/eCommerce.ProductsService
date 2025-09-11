using FluentResults;

namespace eCommerce.ProductsService.Application.Errors;

public class ProductNotFoundError(string message) : Error(message)
{
    public static ProductNotFoundError WithId(Guid productId)
        => new($"Product with ID '{productId}' does not exist.");
}
