using eCommerce.ProductsService.Domain.Entities;
using Mapster;

namespace eCommerce.ProductsService.Application.Messaging;

[AdaptFrom(nameof(Product)), GenerateMapper]
public record ProductUpdatedMessage(
    Guid Id,
    string Name,
    string Category,
    double UnitPrice,
    int QuantityInStock);
