using Bogus;
using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Application.Enums;

namespace eCommerce.ProductsService.Tests.Helpers;

public static class AddProductRequestFactory
{
    private static readonly Faker _faker = new();

    public static AddProductRequest CreateRandom()
        => new(
            _faker.Commerce.ProductName(),
            _faker.Random.Enum<CategoryOptions>().ToString(),
            _faker.Random.Double(1, 100),
            _faker.Random.Int(0, 100));
}
