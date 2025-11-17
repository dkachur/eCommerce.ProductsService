using AutoFixture;
using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Domain.Entities;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.Application.Generated.Mappers;

public class UpdateProductDtoMapperTests
{
    private readonly Fixture _fixture;

    public UpdateProductDtoMapperTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void AdaptToProduct_ShouldMapCorrectly_WhenCalled()
    {
        // Arrange
        var updateProductDto = _fixture.Create<UpdateProductDto>();
        var expected = Product.Restore(
            updateProductDto.Id,
            updateProductDto.Name,
            updateProductDto.Category.ToString(),
            updateProductDto.UnitPrice,
            updateProductDto.QuantityInStock);


        // Act
        var product = updateProductDto.AdaptToProduct();


        // Assert
        product.Should().BeEquivalentTo(expected);
    }
}
