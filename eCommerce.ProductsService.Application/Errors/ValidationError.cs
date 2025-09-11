using FluentResults;

namespace eCommerce.ProductsService.Application.Errors;

public class ValidationError(string message) : Error(message) { }
