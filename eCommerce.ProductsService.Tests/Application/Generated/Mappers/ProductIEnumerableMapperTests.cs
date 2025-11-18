using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Tests.Helpers;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.Application.Generated.Mappers;

public class ProductIEnumerableMapperTests
{
    [Fact]
    public void AdaptToProductDtoList_ShouldMapCorrectly_WhenCalled()
    {
        // Arrange
        var products = Enumerable.Range(0, 10)
            .Select(_ => ProductFactory.CreateRandom())
            .ToList();

        var expected = products
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                Enum.Parse<CategoryOptions>(p.Category),
                p.UnitPrice,
                p.QuantityInStock))
            .ToList();

        // Act
        var productDtos = products.AdaptToProductDtoList();

        // Assert
        productDtos.Should().BeEquivalentTo(expected);
    }  
}
