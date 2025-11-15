using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Domain.Entities;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.Integration.Infrastructure.Repositories;

public partial class ProductsRepositoryTests
{
    public static TheoryData<string[], string[]> GetExistingProductIdsTestData => CreateIdsSelectionTestData();

    [Theory(DisplayName = "GetExistingProductIdsAsync should return only existing IDs from requested IDs")]
    [MemberData(nameof(GetExistingProductIdsTestData))]
    public async Task GetExistingProductIdsAsync_ShouldReturnExistingIds_WhenCalled(string[] existingIds, string[] searchIds)
    {
        // Arrange
        var products = existingIds
            .Select(id => CreateProductWithId(Guid.Parse(id)));

        foreach (var p in products)
            await _repo.AddProductAsync(p);

        var ids = searchIds
            .Select(id => Guid.Parse(id))
            .ToList();

        var expected = ids
            .Intersect(products.Select(p => p.Id))
            .ToList();


        // Act
        var result = await _repo.GetExistingProductIdsAsync(ids);


        // Assert
        result.Count().Should().Be(expected.Count);
        result.Should().BeEquivalentTo(expected);
    }


    private Product CreateProductWithId(Guid id)
        => Product.Restore(
            id,
            _faker.Commerce.ProductName(),
            _faker.Random.Enum<CategoryOptions>().ToString(),
            _faker.Random.Double(1, 100),
            _faker.Random.Int(0, 100));

    private static TheoryData<string[], string[]> CreateIdsSelectionTestData()
    {
        var existingIds = GenerateGuidsAsString(6);
        return new TheoryData<string[], string[]>
        {
            { existingIds, existingIds.Take(3).ToArray() },                             // All IDs from search are present in the database
            { existingIds, new[] { Guid.NewGuid().ToString(), existingIds[0] } },       // Only some IDs from search are present in the database
            { existingIds, existingIds.Take(3).Concat(existingIds.Take(3)).ToArray() }, // Search IDs collection has duplicates
            { existingIds, GenerateGuidsAsString(4) },                                  // None of the IDs from search are present in the database
            { existingIds, [] },                                                        // Search IDs collection is empty
            { [], existingIds },                                                        // Database is empty
        };
    }

    private static string[] GenerateGuidsAsString(int count) => Enumerable.Range(0, count)
        .Select(_ => Guid.NewGuid().ToString())
        .ToArray();
}
