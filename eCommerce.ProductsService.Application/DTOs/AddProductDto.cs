using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Domain.Entities;
using Mapster;

namespace eCommerce.ProductsService.Application.DTOs;

[AdaptTo(typeof(Product)), GenerateMapper]
public record AddProductDto(
    string Name,
    CategoryOptions Category,
    double UnitPrice,
    int QuantityInStock);
