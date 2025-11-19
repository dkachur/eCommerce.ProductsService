using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Tests.Integration.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace eCommerce.ProductsService.Tests.Integration.API;

public partial class ProductsApiTests
{
    [Fact(DisplayName = "PUT /api/products/{id} should update and return product with 200 OK status")]
    public async Task PutProduct_ShouldUpdateProduct_WhenRequest()
    {
        // Arrange
        var productResponses = await SeedProductsAsync(5);
        var randomProduct = _faker.Random.ListItem(productResponses);
        var updateRequest = new UpdateProductRequest(
            randomProduct.Id,
            "new name",
            _faker.Random.Enum<CategoryOptions>(randomProduct.Category).ToString(),
            88.88,
            44);

        var expected = new ProductResponse(
            updateRequest.Id,
            updateRequest.Name,
            Enum.Parse<CategoryOptions>(updateRequest.Category),
            updateRequest.UnitPrice,
            updateRequest.QuantityInStock);


        // Act
        var response = await _client.PutAsJsonAsync($"/api/products/{updateRequest.Id}", updateRequest);
        var productFromApi = await response.Content.ReadFromJsonAsync<ProductResponse>(JsonConfig.Options);


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        productFromApi.Should().BeEquivalentTo(expected);

        var updatedProductRes = await _client.GetAsync($"/api/products/{updateRequest.Id}");
        var updatedProduct = await updatedProductRes.Content.ReadFromJsonAsync<ProductResponse>(JsonConfig.Options);
        updatedProduct.Should().BeEquivalentTo(expected);
    }


    [Fact(DisplayName = "PUT /api/products/{id} should return ProblemDetails and 400 Bad Request when IDs from route and request body do not match")]
    public async Task PutProduct_ShouldReturnProblemDetails_WhenIdsDoNotMatch()
    {
        // Arrange
        var productResponses = await SeedProductsAsync(5);
        var productToUpdate = productResponses[1];
        var updateRequest = new UpdateProductRequest(
            productToUpdate.Id,
            "new name",
            _faker.Random.Enum<CategoryOptions>(productToUpdate.Category).ToString(),
            88.88,
            44);

        var invalidId = productResponses[2].Id;


        // Act
        var response = await _client.PutAsJsonAsync($"/api/products/{invalidId}", updateRequest);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(JsonConfig.Options);


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().NotBeNull();
        problemDetails.Title.Contains("id", StringComparison.InvariantCultureIgnoreCase).Should().BeTrue();
    }


    [Fact(DisplayName = "PUT /api/products/{id} should return ProblemDetails with validation errors and 400 Bad Request when request is invalid")]
    public async Task PutProduct_ShouldReturnProblemDetails_WhenValidationErrors()
    {
        // Arrange
        var productResponses = await SeedProductsAsync(5);
        var productToUpdate = productResponses[1];
        var updateRequest = new UpdateProductRequest(
            Guid.Empty,
            new string('A', 51),
            "test-category",
            -88.88,
            -44);


        // Act
        var response = await _client.PutAsJsonAsync($"/api/products/{updateRequest.Id}", updateRequest);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(JsonConfig.Options);


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        problemDetails.Should().NotBeNull();
        problemDetails.Extensions.Should().ContainKey("errors");

        var errorsElement = problemDetails.Extensions["errors"] as JsonElement?;
        var errors = errorsElement?.Deserialize<Dictionary<string, string[]>>(JsonConfig.Options);

        errors.Should().ContainKeys(
            "Id",
            "Name",
            "Category",
            "UnitPrice",
            "QuantityInStock");
    }


    [Fact(DisplayName = "PUT /api/products/{id} should return 404 Not Found when product does not exist")]
    public async Task PutProduct_ShouldReturn404_WhenProductDoesNotExist()
    {
        // Arrange
        var productResponses = await SeedProductsAsync(5);
        var invalidProduct = new UpdateProductRequest(
            Guid.NewGuid(),
            "invalid product",
            CategoryOptions.Furniture.ToString(),
            88.88,
            44);


        // Act
        var response = await _client.PutAsJsonAsync($"/api/products/{invalidProduct.Id}", invalidProduct);


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
