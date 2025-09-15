using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Enums;
using Mapster;

namespace eCommerce.ProductsService.API.DTOs;

/// <summary>
/// Data transfer object that represents the response product information.
/// </summary>
/// <param name="Id">The unique identifier of the product.</param>
/// <param name="Name">The name of the product.</param>
/// <param name="Category">The category of the product.</param>
/// <param name="UnitPrice">The price of a single product unit.</param>
/// <param name="QuantityInStock">The number of items available in stock.</param>
[AdaptFrom(typeof(ProductDto)), GenerateMapper]
public record ProductResponse(
    Guid Id,
    string Name,
    CategoryOptions Category,
    double UnitPrice,
    int QuantityInStock);
