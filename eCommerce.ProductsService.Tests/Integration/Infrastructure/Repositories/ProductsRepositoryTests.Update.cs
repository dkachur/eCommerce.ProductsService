using AutoFixture;
using eCommerce.ProductsService.Domain.Entities;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.Integration.Infrastructure.Repositories;

public partial class ProductsRepositoryTests
{
    [Fact(DisplayName = "UpdateProductAsync should update product in database when product exists")]
    public async Task UpdateProductAsync_ShouldUpdateProduct_WhenProductExists()
    {
        // Arrange
        var product = _fixture.Create<Product>();
        _ = await _repo.AddProductAsync(product)
           ?? throw new InvalidOperationException("Failed to insert product during Arrange in DeleteProductAsync test.");
        
        var updatedProduct = CreateProductWithId(product.Id);


        // Act
        var result = await _repo.UpdateProductAsync(updatedProduct);


        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(updatedProduct);

        var getRes = await _repo.GetProductByIdAsync(updatedProduct.Id);
        getRes.Should().BeEquivalentTo(updatedProduct);
    }

    [Fact(DisplayName = "UpdateProductAsync should return null when product does not exist in database")]
    public async Task UpdateProductAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var product = _fixture.Create<Product>();


        // Act
        var result = await _repo.UpdateProductAsync(product);


        // Assert
        result.Should().BeNull();

        var getRes = await _repo.GetProductByIdAsync(product.Id);
        getRes.Should().BeNull();
    }
}
