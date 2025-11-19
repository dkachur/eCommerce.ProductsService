using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Tests.Integration.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace eCommerce.ProductsService.Tests.Integration.API;

public partial class ProductsApiTests
{
    [Fact(DisplayName = "GET /api/products should return all products with 200 OK status")]
    public async Task GetProducts_ShouldReturnAllProducts_WhenRequest()
    {
        // Arrange
        var expected = await SeedProductsAsync(10);


        // Act
        var response = await _client.GetAsync("/api/products");
        var productsFromApi = await response.Content.ReadFromJsonAsync<List<ProductResponse>>(JsonConfig.Options);


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        productsFromApi.Should().BeEquivalentTo(expected, opt => opt.WithoutStrictOrdering());
    }
}
