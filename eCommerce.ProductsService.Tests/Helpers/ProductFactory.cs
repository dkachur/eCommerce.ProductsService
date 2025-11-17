using Bogus;
using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Domain.Entities;

namespace eCommerce.ProductsService.Tests.Helpers;

public static class ProductFactory
{
    private static readonly Faker _faker = new();

    public static Product CreateRandom()
    {
        return Product.Restore(
            _faker.Random.Guid(),
            _faker.Commerce.ProductName(),
            _faker.Random.Enum<CategoryOptions>().ToString(),
            _faker.Random.Double(1, 100),
            _faker.Random.Int(0, 100));
    }
}