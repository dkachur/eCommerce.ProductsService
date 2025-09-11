using FluentResults;

namespace eCommerce.ProductsService.Application.Errors;

public class PersistenceError(string message) : Error(message) { }
