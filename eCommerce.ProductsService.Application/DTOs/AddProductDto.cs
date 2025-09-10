namespace eCommerce.ProductsService.Application.DTOs;

public record AddProductDto(
    Guid Id,
    string Name,
    string Category,
    double UnitPrice,
    int QuantityInStock);
