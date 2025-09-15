using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Enums;
using Mapster;

namespace eCommerce.ProductsService.API.DTOs;

/// <summary>
/// Data transfer object for binding the request to add a new product.
/// </summary>
/// <param name="Name">The name of the product.</param>
/// <param name="Category">The category name as string. Will be converted to <see cref="CategoryOptions"/>.</param>
/// <param name="UnitPrice">The price of a single product unit.</param>
/// <param name="QuantityInStock">The number of items available in stock.</param>
[AdaptTo(typeof(AddProductDto)), GenerateMapper]
public record AddProductRequest(
    string Name,
    string Category,
    double UnitPrice,
    int QuantityInStock);