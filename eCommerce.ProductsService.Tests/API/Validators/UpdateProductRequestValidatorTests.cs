using AutoFixture;
using Bogus;
using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.API.Validators;
using eCommerce.ProductsService.Application.Enums;
using FluentAssertions;

namespace eCommerce.ProductsService.Tests.API.Validators;

public class UpdateProductRequestValidatorTests
{
    private readonly UpdateProductRequestValidator _validator;

    private readonly Faker _faker;
    private readonly Fixture _fixture;

    public static TheoryData<string, string, string, double, int, string[]> InvalidProductData =>
        new()
        {
            { Guid.Empty.ToString(), "Bluetooth Headphones", CategoryOptions.Electronics.ToString(), 100, 10, [nameof(UpdateProductRequest.Id)] },                      // Empty ID
            { Guid.NewGuid().ToString(), "", CategoryOptions.Electronics.ToString(), 100, 10, [nameof(UpdateProductRequest.Name)] },                                    // Empty name
            { Guid.NewGuid().ToString(), "   ", CategoryOptions.Electronics.ToString(), 100, 10, [nameof(UpdateProductRequest.Name)] },                                 // Whitespace name
            { Guid.NewGuid().ToString(), new string('A', 51), CategoryOptions.Electronics.ToString(), 100, 10, [nameof(UpdateProductRequest.Name)] },                   // Long name
            { Guid.NewGuid().ToString(), "Bluetooth Headphones", "qwerty", 10, 10, [nameof(UpdateProductRequest.Category)] },                                           // Invalid category
            { Guid.NewGuid().ToString(), "Bluetooth Headphones", CategoryOptions.Electronics.ToString(), 0, 10, [nameof(UpdateProductRequest.UnitPrice)] },             // Zero price
            { Guid.NewGuid().ToString(), "Bluetooth Headphones", CategoryOptions.Electronics.ToString(), 100000, 10, [nameof(UpdateProductRequest.UnitPrice)] },        // High price
            { Guid.NewGuid().ToString(), "Bluetooth Headphones", CategoryOptions.Electronics.ToString(), 100, -1, [nameof(UpdateProductRequest.QuantityInStock)] },     // Negative quantity
            { Guid.NewGuid().ToString(), "Bluetooth Headphones", CategoryOptions.Electronics.ToString(), 100, 1000, [nameof(UpdateProductRequest.QuantityInStock)] },   // High quantity
                            
            { Guid.Empty.ToString(), "", "qwerty", -10, -10,                                                                                                            //
            [                                                                                                                                                           //
                nameof(UpdateProductRequest.Id),                                                                                                                        // 
                nameof(UpdateProductRequest.Name),                                                                                                                      // 
                nameof(UpdateProductRequest.Category),                                                                                                                  // ID, name, category, price and quantity are invalid
                nameof(UpdateProductRequest.UnitPrice),                                                                                                                 //
                nameof(UpdateProductRequest.QuantityInStock),                                                                                                           //
            ] },                                                                                                                                                        //
        };

    public UpdateProductRequestValidatorTests()
    {
        _faker = new Faker();
        _fixture = new Fixture();
        _validator = new UpdateProductRequestValidator();
    }

    [Theory(DisplayName = "ValidateAsync should return validation errors with property name when input is invalid")]
    [MemberData(nameof(InvalidProductData))]
    public async Task ValidateAsync_ShouldFailWithError_WhenInvalidData(string id, string name, string category, double price, int quantity, params string[] invalidPropertyNames)
    {
        // Arrange
        var product = new UpdateProductRequest(Guid.Parse(id), name, category, price, quantity);


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
            .Build<UpdateProductRequest>()
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
