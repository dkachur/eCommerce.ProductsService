using eCommerce.ProductsService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace eCommerce.ProductsService.Tests.Application.Services;

public partial class ProductsServiceTests
{
    [Fact(DisplayName = "GetProductsAsync should return empty list when no products in repo")]
    public async Task GetProductsAsync_ShouldReturnEmptyList_WhenNoProducts()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetProductsAsync())
            .ReturnsAsync([]);


        // Act
        var result = await _service.GetProductsAsync();


        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact(DisplayName = "GetProductsAsync should return products when products exist in repo")]
    public async Task GetProductsAsync_ShouldReturnProducts_WhenProductsExist()
    {
        // Arrange


        // Act
        var result = await _service.GetProductsAsync();


        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        result.Value.Should().BeEquivalentTo(
            _validProducts,
            opt => opt
                .WithoutStrictOrdering()
                .WithAutoConversionFor(
                    objInfo => objInfo.Path.EndsWith($".{nameof(Product.Category)}")
                )
        );
    }
}
