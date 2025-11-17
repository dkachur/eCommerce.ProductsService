using AutoFixture;
using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Application.DTOs;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.API.Generated.Mappers;

public class ProductResponseMapperTests
{
    private readonly Fixture _fixture;

    public ProductResponseMapperTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void AdaptToProductResponse_ShouldMapCorrectly_WhenCalled()
    {
        // Arrange
        var productDto = _fixture.Create<ProductDto>();

        var expected = new ProductResponse(
            productDto.Id,
            productDto.Name,
            productDto.Category,
            productDto.UnitPrice,
            productDto.QuantityInStock);


        // Act
        var productResponse = productDto.AdaptToProductResponse();


        // Assert
        productResponse.Should().BeEquivalentTo(expected);
    }
}
