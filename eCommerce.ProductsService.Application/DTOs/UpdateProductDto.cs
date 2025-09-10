namespace eCommerce.ProductsService.Application.DTOs;

public record UpdateProductDto(
    Guid Id,
    string Name,
    string Category,
    double UnitPrice,
    int QuantityInStock);
