using AutoFixture;
using eCommerce.ProductsService.Domain.Entities;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.Integration.Infrastructure.Repositories;

public partial class ProductsRepositoryTests
{
    [Fact(DisplayName = "DeleteProductAsync should delete product from database if it exists")]
    public async Task DeleteProductAsync_ShouldDeleteProduct_WhenSuccess()
    {
        // Arrange
        var product = _fixture.Create<Product>();
        _ = await _repo.AddProductAsync(product)
            ?? throw new InvalidOperationException("Failed to insert product during Arrange in DeleteProductAsync test.");


        // Act
        var result = await _repo.DeleteProductAsync(product.Id);


        // Assert
        result.Should().BeTrue();

        var getRes = await _repo.GetProductByIdAsync(product.Id);
        getRes.Should().BeNull();
    }
}
