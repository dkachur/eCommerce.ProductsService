using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Domain.Entities;
using Mapster;

namespace eCommerce.ProductsService.Application.DTOs;

/// <summary>
/// Data transfer object that contains information about product.
/// </summary>
/// <param name="Id">The unique identifier of the product.</param>
/// <param name="Name">The name of the product.</param>
/// <param name="Category">The category to which the product belongs.</param>
/// <param name="UnitPrice">The price of a single product unit.</param>
/// <param name="QuantityInStock">The number of items available in stock.</param>
[AdaptFrom(typeof(Product)), GenerateMapper]
public record ProductDto(
    Guid Id,
    string Name,
    CategoryOptions Category,
    double UnitPrice,
    int QuantityInStock);
