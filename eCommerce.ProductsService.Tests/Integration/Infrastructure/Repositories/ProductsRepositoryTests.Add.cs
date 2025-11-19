using AutoFixture;
using eCommerce.ProductsService.Domain.Entities;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.Integration.Infrastructure.Repositories;

public partial class ProductsRepositoryTests
{
    [Fact(DisplayName = "AddProductAsync should insert and return valid product when opertaion is successful")]
    public async Task AddProductAsync_ShouldInsertProduct_WhenSuccess()
    {
        // Arrange
        var product = _fixture.Create<Product>();


        // Act
        var result = await _repo.AddProductAsync(product);


        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(product);

        var inserted = await _repo.GetProductByIdAsync(product.Id);
        inserted.Should().NotBeNull();
        inserted.Should().BeEquivalentTo(product);
    }
}
