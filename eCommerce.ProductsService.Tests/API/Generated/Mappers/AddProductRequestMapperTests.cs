using AutoFixture;
using Bogus;
using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Enums;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.API.Generated.Mappers;

public class AddProductRequestMapperTests
{
    private readonly Faker _faker;
    private readonly Fixture _fixture;

    public AddProductRequestMapperTests()
    {
        _faker = new Faker();
        _fixture = new Fixture();
    }

    [Fact(DisplayName = "AdaptToAddProductDto should map from AddProductRequest to AddProductDto correctly")]
    public void AdaptToAddProductDto_ShouldMapCorrectly_WhenCalled()
    {
        // Arrange 
        var addProductRequest = _fixture
            .Build<AddProductRequest>()
            .With(p => p.Category, _faker.Random.Enum<CategoryOptions>().ToString())
            .Create();

        var expected = new AddProductDto(
            addProductRequest.Name,
            Enum.Parse<CategoryOptions>(addProductRequest.Category),
            addProductRequest.UnitPrice,
            addProductRequest.QuantityInStock);


        // Act
        var addProductDto = addProductRequest.AdaptToAddProductDto();


        // Assert
        addProductDto.Should().BeEquivalentTo(expected);
    }
}
