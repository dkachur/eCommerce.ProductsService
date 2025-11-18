using AutoFixture;
using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Domain.Entities;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.Application.Generated.Mappers;

public class AddProductDtoMapperTests
{
    private readonly Fixture _fixture;

    public AddProductDtoMapperTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void AdaptToProduct_ShouldMapCorrectly_WhenCalled()
    {
        // Arrange
        var addProductDto = _fixture.Create<AddProductDto>();
        var expected = Product.New(
            addProductDto.Name,
            addProductDto.Category.ToString(),
            addProductDto.UnitPrice,
            addProductDto.QuantityInStock);


        // Act
        var product = addProductDto.AdaptToProduct();


        // Assert
        product.Should().BeEquivalentTo(expected, opt => opt.Excluding(p => p.Id));
    }
}
