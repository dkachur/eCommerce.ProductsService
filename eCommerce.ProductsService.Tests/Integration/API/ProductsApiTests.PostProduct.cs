using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Tests.Helpers;
using eCommerce.ProductsService.Tests.Integration.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace eCommerce.ProductsService.Tests.Integration.API;

public partial class ProductsApiTests
{
    [Fact(DisplayName = "POST /api/products should add product and return 201 Created")]
    public async Task PostProduct_ShouldReturnCreatedProduct_WhenRequest()
    {
        // Arrange
        var request = AddProductRequestFactory.CreateRandom();
        var expected = new ProductResponse(
            Guid.Empty,
            request.Name,
            Enum.Parse<CategoryOptions>(request.Category),
            request.UnitPrice,
            request.QuantityInStock);


        // Act
        var response = await _client.PostAsJsonAsync("/api/products", request, JsonConfig.Options);
        var created = await response.Content.ReadFromJsonAsync<ProductResponse>(JsonConfig.Options);


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        created.Should().BeEquivalentTo(expected, opt => opt.Excluding(p => p.Id));
        created.Id.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "POST /api/products should return ProblemDetails with validation errors and 400 Bad Request when request is invalid")]
    public async Task PostProduct_ShouldReturnBadRequest_WhenValidationErrors()
    {
        // Arrange
        var request = AddProductRequestFactory.CreateRandom() with 
        { 
            Name = new string('A', 51),
            Category = "test-category",
            UnitPrice = -100,
            QuantityInStock = -1
        };


        // Act
        var response = await _client.PostAsJsonAsync("/api/products", request, JsonConfig.Options);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(JsonConfig.Options);


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        problemDetails.Should().NotBeNull();
        problemDetails.Extensions.Should().ContainKey("errors");

        var errorsElement = problemDetails.Extensions["errors"] as JsonElement?;
        var errors = errorsElement?.Deserialize<Dictionary<string, string[]>>(JsonConfig.Options);

        errors.Should().ContainKeys(
            "Name",
            "Category",
            "UnitPrice",
            "QuantityInStock");
    }
}
