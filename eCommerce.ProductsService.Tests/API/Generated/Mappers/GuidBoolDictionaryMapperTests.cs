using AutoFixture;
using eCommerce.ProductsService.API.DTOs;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.API.Generated.Mappers;

public class GuidBoolDictionaryMapperTests
{
    private readonly Fixture _fixture;

    public GuidBoolDictionaryMapperTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void AdaptToProductExistResponseList_ShouldMapCorrectly_WhenCalled()
    {
        // Arrange
        var dict = _fixture
            .CreateMany<KeyValuePair<Guid, bool>>(10)
            .ToDictionary();

        var expected = dict
            .Select(kvp => new ProductExistResponse(kvp.Key, kvp.Value))
            .ToList();


        // Act
        var productExistList = dict.AdaptToProductExistResponseList();


        // Assert
        productExistList.Should().BeEquivalentTo(expected);
    }
}
