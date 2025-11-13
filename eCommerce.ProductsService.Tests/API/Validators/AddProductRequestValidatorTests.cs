using AutoFixture;
using Bogus;
using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.API.Validators;
using eCommerce.ProductsService.Application.Enums;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.API.Validators;

public class AddProductRequestValidatorTests
{
    private readonly AddProductRequestValidator _validator;

    private readonly Faker _faker;
    private readonly Fixture _fixture;

    public static TheoryData<string, string, double, int, string[]> InvalidProductData =>
        new()
        {
            { "", CategoryOptions.Electronics.ToString(), 100, 10, [nameof(AddProductRequest.Name)] },                                      // Empty name
            { "   ", CategoryOptions.Electronics.ToString(), 100, 10, [nameof(AddProductRequest.Name)] },                                   // Whitespace name
            { new string('A', 51), CategoryOptions.Electronics.ToString(), 100, 10, [nameof(AddProductRequest.Name)] },                     // Long name
            { "Bluetooth Headphones", "qwerty", 100, 10, [nameof(AddProductRequest.Category)] },                                            // Invalid category
            { "Bluetooth Headphones", CategoryOptions.Electronics.ToString(), 0, 10, [nameof(AddProductRequest.UnitPrice)] },               // Zero price
            { "Bluetooth Headphones", CategoryOptions.Electronics.ToString(), 100000, 10, [nameof(AddProductRequest.UnitPrice)] },          // High price
            { "Bluetooth Headphones", CategoryOptions.Electronics.ToString(), 100, -1, [nameof(AddProductRequest.QuantityInStock)] },       // Negative quantity
            { "Bluetooth Headphones", CategoryOptions.Electronics.ToString(), 100, 1000, [nameof(AddProductRequest.QuantityInStock)] },     // High quantity

            { "", "qwerty", -10, -10,                                                                                                       //
            [                                                                                                                               //
                nameof(AddProductRequest.Name),                                                                                             // 
                nameof(AddProductRequest.Category),                                                                                         // Name, category, price and quantity are invalid
                nameof(AddProductRequest.UnitPrice),                                                                                        //
                nameof(AddProductRequest.QuantityInStock),                                                                                  //
            ] },                                                                                                                            //
        };

    public AddProductRequestValidatorTests()
    {
        _faker = new Faker();
        _fixture = new Fixture();
        _validator = new AddProductRequestValidator();
    }

    [Theory(DisplayName = "ValidateAsync should return validation errors with property name when input is invalid")]
    [MemberData(nameof(InvalidProductData))]
    public async Task ValidateAsync_ShouldFailWithError_WhenInvalidData(string name, string category, double price, int quantity, params string[] invalidPropertyNames)
    {
        // Arrange
        var product = new AddProductRequest(name, category, price, quantity);


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
            .Build<AddProductRequest>()
            .With(p => p.Name, () => _faker.Random.String2(20))
            .With(p => p.Category, () => _faker.Random.Enum<CategoryOptions>().ToString())
            .With(p => p.UnitPrice, () => _faker.Random.Double(1, 99999))
            .With(p => p.QuantityInStock, () => _faker.Random.Int(0, 999))
            .Create();


        // Act
        var result = await _validator.ValidateAsync(product);


        // Assert
        result.IsValid.Should().BeTrue();
    }
}
