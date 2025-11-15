using AutoFixture;
using eCommerce.ProductsService.Domain.Entities;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.Integration.Infrastructure.Repositories;

public partial class ProductsRepositoryTests
{
    [Fact(DisplayName = "GetProductByIdAsync should retrieve product from database if it exists")]
    public async Task GetProductByIdAsync_ShouldRetrieveProduct_WhenSuccess()
    {
        // Arrange
        var product = _fixture.Create<Product>();
        _ = await _repo.AddProductAsync(product)
            ?? throw new InvalidOperationException("Failed to insert product during Arrange in GetProductByIdAsync test.");


        // Act
        var result = await _repo.GetProductByIdAsync(product.Id);


        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(product);
    }

    [Fact(DisplayName = "GetProductByIdAsync should return null when product does not exist in the database")]
    public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();


        // Act
        var result = await _repo.GetProductByIdAsync(id);


        // Assert
        result.Should().BeNull();
    }
}
