using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Domain.Entities;
using Mapster;

namespace eCommerce.ProductsService.Application.DTOs;

[AdaptTo(typeof(Product)), GenerateMapper]
public record UpdateProductDto(
    Guid Id,
    string Name,
    CategoryOptions Category,
    double UnitPrice,
    int QuantityInStock);
