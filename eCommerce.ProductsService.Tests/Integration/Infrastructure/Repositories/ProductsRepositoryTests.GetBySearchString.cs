using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Domain.Entities;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.Integration.Infrastructure.Repositories;

public partial class ProductsRepositoryTests
{
    public static TheoryData<string[], string> GetBySearchStringTestData =>
        new()
        {
            { 
                [
                    "Computer for home",
                    "Office chair",
                    "Home lamp",
                    "Dining home table",
                    "Gaming chair",
                    "Home charger",
                    "Refrigerator",
                    "Power bank",
                    "Home chair black",
                    "Chair white",
                ], 
                                            // Some products match and search must be case-insensitive
                "hOMe"                      // Result must not be empty
            },

            {
                [
                    "Computer",
                    "Office chair",
                    "Home lamp",
                    "Dining home table",
                    "Gaming chair",
                    "Home charger",
                    "Refrigerator",
                    "Power bank",
                    "Home chair black",
                    "Chair white",
                ],
                                            // None products match
                "plate"                     // Result must be empty
            },

            {
                [
                    
                ],
                                            // No products in database
                "accessories"               // Result must be empty
            },
        };

    [Theory(DisplayName = "GetBySearchStringAsync should retrieve searched products from database")]
    [MemberData(nameof(GetBySearchStringTestData))]
    public async Task GetBySearchStringAsync_ShouldRetrieveProducts_WhenMatch(string[] productNames, string search)
    {
        // Arrange
        var products = productNames
            .Select(n => CreateProductWithName(n))
            .ToList();

        foreach (var p in products)
            await _repo.AddProductAsync(p);

        var expected = products
            .Where(p => ContainsSearch(p, search))
            .ToList();


        // Act
        var result = await _repo.GetBySearchStringAsync(search);
        var resultList = result.ToList();


        // Assert
        resultList.Should().BeEquivalentTo(expected, opt => opt.WithoutStrictOrdering());
    }


    private Product CreateProductWithName(string name)
        => Product.New(
            name,
            _faker.Random.Enum<CategoryOptions>().ToString(),
            _faker.Random.Double(1, 100),
            _faker.Random.Int(0, 100));

    private static bool ContainsSearch(Product p, string search)
        => p.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase)
            || p.Category.Contains(search, StringComparison.InvariantCultureIgnoreCase);
}
