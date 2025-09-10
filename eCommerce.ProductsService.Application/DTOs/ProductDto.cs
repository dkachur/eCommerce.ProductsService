namespace eCommerce.ProductsService.Application.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string Category,
    double UnitPrice,
    int QuantityInStock);
