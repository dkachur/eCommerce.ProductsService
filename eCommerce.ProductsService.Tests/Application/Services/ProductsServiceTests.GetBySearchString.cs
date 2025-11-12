using eCommerce.ProductsService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace eCommerce.ProductsService.Tests.Application.Services;

public partial class ProductsServiceTests
{
    [Fact(DisplayName = "GetBySearchStringAsync should return empty list when no products found")]
    public async Task GetBySearchStringAsync_ShouldReturnEmptyList_WhenNoProductsFound()
    {
        // Arrange
        var searchString = "no-products";

        _repoMock
            .Setup(r => r.GetBySearchStringAsync(It.IsAny<string>()))
            .ReturnsAsync([]);
        

        // Act
        var result = await _service.GetBySearchStringAsync(searchString);


        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact(DisplayName = "GetBySearchStringAsync should return all products from repo when input is empty")]
    public async Task GetBySearchStringAsync_ShouldReturnAllProducts_WhenEmptyInput()
    {
        // Arrange
        var searchString = "";


        // Act
        var result = await _service.GetBySearchStringAsync(searchString);


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

    [Fact(DisplayName = "GetBySearchStringAsync should return products by search string")]
    public async Task GetBySearchStringAsync_ShouldReturnProductsBySearchString_WhenValidInput()
    {
        // Arrange
        var searchString = "chair";


        // Act
        var result = await _service.GetBySearchStringAsync(searchString);


        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        result.Value.Should().BeEquivalentTo(
            _productsFromSearch,
            opt => opt
                .WithoutStrictOrdering()
                .WithAutoConversionFor(
                    objInfo => objInfo.Path.EndsWith($".{nameof(Product.Category)}")
                )
        );
    }
}
