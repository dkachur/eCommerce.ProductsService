using AutoFixture;
using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Domain.Entities;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.Integration.Infrastructure.Repositories;

public partial class ProductsRepositoryTests
{
    [Theory(DisplayName = "GetProductsAsync should return all products from database when called")]
    [InlineData(10)]
    [InlineData(0)]
    [InlineData(100)]
    public async Task GetProductsAsync_ShouldReturnAllProducts_WhenCalled(int productsCount) 
    {
        // Arrange
        var products = Enumerable.Range(0, productsCount)
            .Select(_ => _fixture.Create<Product>())
            .ToList();

        foreach (var p in products)
            await _repo.AddProductAsync(p);


        // Act
        var result = await _repo.GetProductsAsync();


        // Assert
        result.Should().BeEquivalentTo(products, opt => opt.WithoutStrictOrdering());
    }
}
