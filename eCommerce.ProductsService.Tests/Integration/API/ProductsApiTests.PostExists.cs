using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Tests.Integration.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace eCommerce.ProductsService.Tests.Integration.API;

public partial class ProductsApiTests
{
    [Fact(DisplayName = "POST /api/products/exists should return product existence infos")]
    public async Task PostExists_ShouldReturnExistingInfo_WhenRequest()
    {
        // Arrange
        var productResponses = await SeedProductsAsync(10);
        var existingIds = _faker.Random.ListItems(productResponses, 5)
            .Select(p => p.Id)
            .ToList();

        var invalidIds = Enumerable.Range(0, 3)
            .Select(_ => Guid.NewGuid())
            .ToList();

        var requestIds = _faker.Random.Shuffle(existingIds.Concat(invalidIds))
            .ToList();

        var expected = requestIds
            .Select(id => new ProductExistResponse(id, existingIds.Contains(id)))
            .ToList();


        // Act
        var response = await _client.PostAsJsonAsync($"/api/products/exists", requestIds);
        var existInfos = await response.Content.ReadFromJsonAsync<List<ProductExistResponse>>(JsonConfig.Options);


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        existInfos.Should().BeEquivalentTo(expected, opt => opt.WithoutStrictOrdering());
    }
}
