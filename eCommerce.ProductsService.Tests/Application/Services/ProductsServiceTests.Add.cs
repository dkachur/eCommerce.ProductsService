using AutoFixture;
using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Errors;
using eCommerce.ProductsService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace eCommerce.ProductsService.Tests.Application.Services;

public partial class ProductsServiceTests
{
    [Fact(DisplayName = "AddProductAsync should return ValidationError when DTO is invalid")]
    public async Task AddProductAsync_ShouldReturnValidationError_WhenDtoIsInvalid()
    {
        // Arrange
        var product = _fixture.Create<AddProductDto>();
        SetupValidatorMock(_addValidatorMock, nameof(AddProductDto.Name), "Product is invalid");


        // Act
        var result = await _service.AddProductAsync(product);


        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ValidationError>();
    }

    [Fact(DisplayName = "AddProductAsync should return PersistenceError when repository cannot add product")]
    public async Task AddProductAsync_ShouldReturnPersistenceError_WhenRepositoryCannotAddProduct()
    {
        // Arrange
        var product = _fixture.Create<AddProductDto>();
        _repoMock
            .Setup(r => r.AddProductAsync(It.IsAny<Product>()))
            .ReturnsAsync(null as Product);


        // Act
        var result = await _service.AddProductAsync(product);


        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<PersistenceError>();
    }

    [Fact(DisplayName = "AddProductAsync should return ProductDto when insertion is successful")]
    public async Task AddProduct_ShouldReturnProductDto_WhenSuccess()
    {
        // Arrange
        var product = _fixture.Create<AddProductDto>();


        // Act
        var result = await _service.AddProductAsync(product);


        // Arrange
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(product, opt =>
            opt.ExcludingMissingMembers());
    }
}
