using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Domain.Entities;
using Mapster;

namespace eCommerce.ProductsService.Application.DTOs;

/// <summary>
/// Data transfer object that contains information required to update an existing product.
/// </summary>
/// <param name="Id">The unique identifier of the product. Used to locate the product that should be updated.</param>
/// <param name="Name">The name of the product.</param>
/// <param name="Category">The category to which the product belongs.</param>
/// <param name="UnitPrice">The price of a single product unit.</param>
/// <param name="QuantityInStock">The number of items available in stock.</param>
[AdaptTo(typeof(Product)), GenerateMapper]
public record UpdateProductDto(
    Guid Id,
    string Name,
    CategoryOptions Category,
    double UnitPrice,
    int QuantityInStock);
