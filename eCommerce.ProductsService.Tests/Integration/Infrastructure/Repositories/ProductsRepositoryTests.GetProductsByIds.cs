using FluentAssertions;

namespace eCommerce.ProductsService.Tests.Integration.Infrastructure.Repositories;

public partial class ProductsRepositoryTests
{
    public static TheoryData<string[], string[]> GetProductsByIdsTestData => CreateIdsSelectionTestData();

    [Theory(DisplayName = "GetProductsByIdsAsync should return only existing products from requested IDs")]
    [MemberData(nameof(GetProductsByIdsTestData))]
    public async Task GetProductsByIdsAsync_ShouldReturnExistingProducts_WhenCalled(string[] existingIds, string[] searchIds)
    {
        // Arrange
        var products = existingIds
            .Select(id => CreateProductWithId(Guid.Parse(id)))
            .ToList();

        foreach (var p in products)
            await _repo.AddProductAsync(p);

        var ids = searchIds
            .Select(id => Guid.Parse(id))
            .ToList();

        var expected = products
            .Where(p => ids.Contains(p.Id))
            .ToList();


        // Act
        var result = await _repo.GetProductsByIdsAsync(ids);


        // Assert
        result.Should().BeEquivalentTo(expected, opt => opt.WithoutStrictOrdering());
    }
}
