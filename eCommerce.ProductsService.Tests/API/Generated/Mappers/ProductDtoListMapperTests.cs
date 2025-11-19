using AutoFixture;
using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Application.DTOs;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.API.Generated.Mappers;

public class ProductDtoListMapperTests
{
    private readonly Fixture _fixture;

    public ProductDtoListMapperTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void AdaptToProductResponseList_ShouldMapCorrectly_WhenCalled()
    {
        // Arrange
        var productDtoList = _fixture
            .CreateMany<ProductDto>()
            .ToList();

        var expected = productDtoList
            .Select(p => new ProductResponse(
                p.Id,
                p.Name,
                p.Category,
                p.UnitPrice,
                p.QuantityInStock));


        // Act
        var productResponseList = productDtoList.AdaptToProductResponseList();


        // Assert
        productResponseList.Should().BeEquivalentTo(expected);
    }
}
