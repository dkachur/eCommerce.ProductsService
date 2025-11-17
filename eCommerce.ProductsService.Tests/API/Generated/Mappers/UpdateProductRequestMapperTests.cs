using AutoFixture;
using Bogus;
using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Enums;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.API.Generated.Mappers;

public class UpdateProductRequestMapperTests
{
    private readonly Faker _faker;
    private readonly Fixture _fixture;

    public UpdateProductRequestMapperTests()
    {
        _faker = new Faker();
        _fixture = new Fixture();
    }

    [Fact]
    public void AdaptToUpdateProductDto_ShouldMapCorrectly_WhenCalled()
    {
        // Arrange
        var updateProductRequest = _fixture
            .Build<UpdateProductRequest>()
            .With(p => p.Category, _faker.Random.Enum<CategoryOptions>().ToString())
            .Create();

        var expected = new UpdateProductDto(
            updateProductRequest.Id,
            updateProductRequest.Name,
            Enum.Parse<CategoryOptions>(updateProductRequest.Category),
            updateProductRequest.UnitPrice,
            updateProductRequest.QuantityInStock);


        // Act
        var updateProductDto = updateProductRequest.AdaptToUpdateProductDto();


        // Assert
        updateProductDto.Should().BeEquivalentTo(expected);
    }
}
