using eCommerce.ProductsService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace eCommerce.ProductsService.Tests.Application.Services;

public partial class ProductsServiceTests
{
    private const int ProductsByIdsSomeFoundCount = 4;

    [Theory(DisplayName = "GetProductsByIdsAsync should return empty list when input is empty or no products found")]
    [InlineData(0)]                     // Input is empty list
    [InlineData(ProductsByIdsCount)]    // Input is not empty
    public async Task GetProductsByIdsAsync_ShouldReturnEmptyList_WhenEmptyInputOrNoFound(int inputCount)
    {
        // Arrange 
        var ids = Enumerable
            .Range(0, inputCount)
            .Select(_ => Guid.NewGuid())
            .ToList();

        _repoMock
            .Setup(r => r.GetProductsByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync([]);


        // Act
        var result = await _service.GetProductsByIdsAsync(ids);


        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Theory(DisplayName = "GetProductsByIdsAsync should return correct products depending on found IDs")]
    [InlineData(ProductsByIdsCount, ProductsByIdsCount)]            // all ids found 
    [InlineData(ProductsByIdsCount, ProductsByIdsSomeFoundCount)]   // only some ids found
    public async Task GetProductsByIdsAsync_ShouldReturnCorrectProducts_WhenRepoReturnsAllOrSomeProducts(int inputCount, int foundCount)
    {
        // Arrange 
        var ids = Enumerable
            .Range(0, inputCount)
            .Select(_ => Guid.NewGuid())
            .ToList();

        var products = _productsByIds.Take(foundCount).ToList();
        _repoMock
            .Setup(r => r.GetProductsByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(products);


        // Act
        var result = await _service.GetProductsByIdsAsync(ids);


        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        result.Value.Should().BeEquivalentTo(
            products,
            opt => opt
                .WithoutStrictOrdering()
                .WithAutoConversionFor(
                    objInfo => objInfo.Path.EndsWith($".{nameof(Product.Category)}")
                )
        );
    }
}
