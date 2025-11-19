using FluentAssertions;
using System.Net;

namespace eCommerce.ProductsService.Tests.Integration.API;

public partial class ProductsApiTests
{
    [Fact(DisplayName = "DELETE /api/products/{id} should delete product and return 204 No Content when product exists")]
    public async Task DeleteProduct_ShouldReturn204_WhenRequest()
    {
        // Arrange
        var productResponses = await SeedProductsAsync(5);
        var productIdToDelete = productResponses[3].Id;


        // Act
        var response = await _client.DeleteAsync($"/api/products/{productIdToDelete}");


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var deletedProductRes = await _client.GetAsync($"/api/products/{productIdToDelete}");
        deletedProductRes.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "DELETE /api/products/{id} should return 404 Not Found when product does not exist")]
    public async Task DeleteProduct_ShouldReturn404_WhenProductDoesNotExist()
    {
        // Arrange
        var productResponses = await SeedProductsAsync(5);
        var invalidId = Guid.NewGuid();


        // Act
        var response = await _client.DeleteAsync($"/api/products/{invalidId}");


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
