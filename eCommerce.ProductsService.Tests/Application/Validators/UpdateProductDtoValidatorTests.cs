using AutoFixture;
using Bogus;
using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Application.Validators;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.Application.Validators;

public class UpdateProductDtoValidatorTests
{
    private readonly UpdateProductDtoValidator _validator;
    private readonly Faker _faker;
    private readonly Fixture _fixture;

    public static TheoryData<string, string, CategoryOptions, double, int, string[]> InvalidProductDataTheory =>
        new()
        {
            { Guid.Empty.ToString(), "Bluetooth Headphones", CategoryOptions.Electronics, 100, 10, [nameof(UpdateProductDto.Id)] },                     // Empty ID
            { Guid.NewGuid().ToString(), "", CategoryOptions.Electronics, 100, 10, [nameof(UpdateProductDto.Name)] },                                   // Empty name
            { Guid.NewGuid().ToString(), "   ", CategoryOptions.Electronics, 100, 10, [nameof(UpdateProductDto.Name)] },                                // Whitespace name
            { Guid.NewGuid().ToString(), new string('A', 51), CategoryOptions.Electronics, 100, 10, [nameof(UpdateProductDto.Name)] },                  // Long name
            { Guid.NewGuid().ToString(), "Bluetooth Headphones", CategoryOptions.Electronics, 0, 10, [nameof(UpdateProductDto.UnitPrice)] },            // Zero price
            { Guid.NewGuid().ToString(), "Bluetooth Headphones", CategoryOptions.Electronics, 100000, 10, [nameof(UpdateProductDto.UnitPrice)] },       // High price
            { Guid.NewGuid().ToString(), "Bluetooth Headphones", CategoryOptions.Electronics, 100, -1, [nameof(UpdateProductDto.QuantityInStock)] },    // Negative quantity
            { Guid.NewGuid().ToString(), "Bluetooth Headphones", CategoryOptions.Electronics, 100, 1000, [nameof(UpdateProductDto.QuantityInStock)] },  // High quantity

            { Guid.Empty.ToString(), "", CategoryOptions.Electronics, -10, -10,                                                                         //
            [                                                                                                                                           //
                nameof(UpdateProductDto.Id),                                                                                                            //
                nameof(UpdateProductDto.Name),                                                                                                          // ID, name, price and quantity are invalid
                nameof(UpdateProductDto.UnitPrice),                                                                                                     //
                nameof(UpdateProductDto.QuantityInStock),                                                                                               //
            ] },                                                                                                                                        //
        };

    public UpdateProductDtoValidatorTests()
    {
        _faker = new Faker();
        _fixture = new Fixture();
        _validator = new UpdateProductDtoValidator();
    }

    [Theory(DisplayName = "ValidateAsync should return validation errors with property name when input is invalid")]
    [MemberData(nameof(InvalidProductDataTheory))]
    public async Task ValidateAsync_ShouldFailWithError_WhenInvalidData(string guid, string name, CategoryOptions category, double price, int quantity, params string[] invalidPropertyNames)
    {
        // Arrange
        var product = new UpdateProductDto(Guid.Parse(guid), name, category, price, quantity);


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
            .Build<UpdateProductDto>()
            .With(p => p.UnitPrice, () => _faker.Random.Double(1, 99999))
            .With(p => p.QuantityInStock, () => _faker.Random.Int(0, 999))
            .Create();


        // Act
        var result = await _validator.ValidateAsync(product);


        // Assert
        result.IsValid.Should().BeTrue();
    }

}
