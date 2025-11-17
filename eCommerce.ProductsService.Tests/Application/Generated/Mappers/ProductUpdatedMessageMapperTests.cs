using eCommerce.ProductsService.Application.Messaging;
using eCommerce.ProductsService.Tests.Helpers;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.Application.Generated.Mappers;

public class ProductUpdatedMessageMapperTests
{
    [Fact]
    public void AdaptToProductUpdatedMessage_ShouldMapCorrectly_WhenCalled()
    {
        // Arrange
        var product = ProductFactory.CreateRandom();
        var expected = new ProductUpdatedMessage(
            product.Id, 
            product.Name, 
            product.Category, 
            product.UnitPrice, 
            product.QuantityInStock);


        // Act
        var productUpdatedMessage = product.AdaptToProductUpdatedMessage();


        // Assert
        productUpdatedMessage.Should().BeEquivalentTo(expected);
    }
}
