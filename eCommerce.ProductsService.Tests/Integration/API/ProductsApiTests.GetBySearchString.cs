using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Tests.Integration.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace eCommerce.ProductsService.Tests.Integration.API;

public partial class ProductsApiTests
{
    [Fact(DisplayName = "GET /api/products/search/{searchString} should return products which Name or Category contain search string")]
    public async Task GetProductBySearchString_ShouldReturnCorrectProducts_WhenRequest()
    {
        // Arrange
        var searchString = "ele";

        var additionalProducts = new List<AddProductRequest>
        {
            new("Electric watches", CategoryOptions.Accessories.ToString(), 129, 4),
            new("Elementary school book", CategoryOptions.HomeAppliances.ToString(), 2, 56),
            new("Chemistry elements table", CategoryOptions.HomeAppliances.ToString(), 3.49, 20),
            new("Ceramic elements mug", CategoryOptions.Furniture.ToString(), 12, 8),
        };

        var productResponses = await SeedProductsAsync(10, additionalProducts);

        var expected = productResponses
            .Where(p => 
                p.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
                || p.Category.ToString().Contains(searchString, StringComparison.InvariantCultureIgnoreCase))
            .ToList();


        // Act
        var response = await _client.GetAsync($"/api/products/search/{searchString}");
        var productsFromApi = await response.Content.ReadFromJsonAsync<List<ProductResponse>>(JsonConfig.Options);


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        productsFromApi.Should().BeEquivalentTo(expected, opt => opt.WithoutStrictOrdering());
    }
}
