using AutoFixture;
using Bogus;
using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Application.Validators;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.Application.Validators;

public class AddProductDtoValidatorTests
{
    private readonly AddProductDtoValidator _validator;

    private readonly Faker _faker;
    private readonly Fixture _fixture;

    public AddProductDtoValidatorTests()
    {
        _faker = new Faker();
        _fixture = new Fixture();
        _validator = new AddProductDtoValidator();
    }

    [Theory(DisplayName = "ValidateAsync should return validation errors with property name when input is invalid")]
    [InlineData("", CategoryOptions.Electronics, 100, 10, nameof(AddProductDto.Name))]                                      // Empty name
    [InlineData("    ", CategoryOptions.Electronics, 100, 10, nameof(AddProductDto.Name))]                                  // Whitespace name
    [InlineData("Bluetooth Headphones", CategoryOptions.Electronics, 0, 10, nameof(AddProductDto.UnitPrice))]               // Zero price
    [InlineData("Bluetooth Headphones", CategoryOptions.Electronics, 100000, 10, nameof(AddProductDto.UnitPrice))]          // High price
    [InlineData("Bluetooth Headphones", CategoryOptions.Electronics, 100, -1, nameof(AddProductDto.QuantityInStock))]       // Negative quantity
    [InlineData("Bluetooth Headphones", CategoryOptions.Electronics, 100, 1000, nameof(AddProductDto.QuantityInStock))]     // High quantity
    [InlineData("", CategoryOptions.Electronics, -10, -10, nameof(AddProductDto.Name), nameof(AddProductDto.UnitPrice), nameof(AddProductDto.QuantityInStock))]     // Name, price and quantity are invalid
    public async Task ValidateAsync_ShouldFailWithError_WhenInvalidData(string name, CategoryOptions category, double price, int quantity, params string[] invalidPropertyNames)
    {
        // Arrange
        var product = new AddProductDto(name, category, price, quantity);


        // Act
        var result = await _validator.ValidateAsync(product);


        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        foreach (var prop in invalidPropertyNames)
            result.Errors.Should().Contain(e => e.PropertyName == prop);
    }

    [Fact(DisplayName = "ValidateAsync should succeed when input is valid")]
    public async Task ValidateAsync_ShouldSuccess_WhenValidData()
    {
        // Arrange
        var product = _fixture
            .Build<AddProductDto>()
            .With(p => p.UnitPrice, () => _faker.Random.Double(1, 99999))
            .With(p => p.QuantityInStock, () => _faker.Random.Int(0, 999))
            .Create();


        // Act
        var result = await _validator.ValidateAsync(product);


        // Assert
        result.IsValid.Should().BeTrue();
    }
}
