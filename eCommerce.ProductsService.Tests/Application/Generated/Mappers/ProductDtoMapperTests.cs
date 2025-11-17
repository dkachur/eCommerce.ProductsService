using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Tests.Helpers;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.Application.Generated.Mappers;

public class ProductDtoMapperTests
{
    [Fact]
    public void AdaptToProductDto_ShouldMapCorrectly_WhenCalled()
    {
        // Arrange
        var product = ProductFactory.CreateRandom();          
            
        var expected = new ProductDto(
            product.Id,
            product.Name,
            Enum.Parse<CategoryOptions>(product.Category),
            product.UnitPrice,
            product.QuantityInStock);

        // Act
        var productDto = product.AdaptToProductDto();


        // Assert
        productDto.Should().BeEquivalentTo(expected);
    }
}
