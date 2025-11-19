using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Tests.Integration.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace eCommerce.ProductsService.Tests.Integration.API;

public partial class ProductsApiTests
{
    [Fact(DisplayName = "GET /api/products/{id} should return product with specified id when it exists")]
    public async Task GetProductById_ShouldReturnCorrectProduct_WhenRequest()
    {
        // Arrange
        var productResponses = await SeedProductsAsync(10);
        var expected = _faker.Random.ListItem(productResponses);


        // Act
        var response = await _client.GetAsync($"/api/products/{expected.Id}");
        var productFromApi = await response.Content.ReadFromJsonAsync<ProductResponse>(JsonConfig.Options);


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        productFromApi.Should().BeEquivalentTo(expected);
    }

    [Fact(DisplayName = "GET /api/products/{id} should return 404 Not Found when product with specified id does not exist")]
    public async Task GetProductById_ShouldReturnNotFound_WhenProductDoesNotExists()
    {
        // Arrange


        // Act
        var response = await _client.GetAsync($"/api/products/{Guid.NewGuid()}");


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
