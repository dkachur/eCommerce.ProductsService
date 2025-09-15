using eCommerce.ProductsService.Domain.Entities;
using Mapster;

namespace eCommerce.ProductsService.Application.DTOs;

[AdaptFrom(typeof(Product)), GenerateMapper]
public record ProductDto(
    Guid Id,
    string Name,
    CategoryOptions Category,
    double UnitPrice,
    int QuantityInStock);
