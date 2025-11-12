using AutoFixture;
using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Errors;
using eCommerce.ProductsService.Application.Messaging;
using eCommerce.ProductsService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace eCommerce.ProductsService.Tests.Application.Services;

public partial class ProductsServiceTests
{
    [Fact(DisplayName = "UpdateProductAsync should return ValidationError when DTO is invalid")]
    public async Task UpdateProductAsync_ShouldReturnValidationError_WhenDtoIsInvalid()
    {
        // Arrange
        var product = _fixture.Create<UpdateProductDto>();
        SetupValidatorMock(_updateValidatorMock, nameof(UpdateProductDto.Name), "Product is invalid");


        // Act
        var result = await _service.UpdateProductAsync(product);


        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ValidationError>();
    }

    [Fact(DisplayName = "UpdateProductAsync should return ProductNotFoundError when product does not exist")]
    public async Task UpdateProductAsync_ShouldReturnProductNotFoundError_WhenProductDoesNotExist()
    {
        // Arrange
        var product = _fixture.Create<UpdateProductDto>();
        _repoMock
            .Setup(r => r.GetProductByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Product);


        // Act
        var result = await _service.UpdateProductAsync(product);


        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ProductNotFoundError>();
    }

    [Fact(DisplayName = "UpdateProductAsync should return PersistenceError when repository cannot update product")]
    public async Task UpdateProductAsync_ShouldReturnPersistenceError_WhenRepositoryCannotUpdateProduct()
    {
        // Arrange
        var product = _fixture.Create<UpdateProductDto>();

        _repoMock
            .Setup(r => r.UpdateProductAsync(It.IsAny<Product>()))
            .ReturnsAsync(null as Product);


        // Act 
        var result = await _service.UpdateProductAsync(product);


        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<PersistenceError>();
    }

    [Fact(DisplayName = "UpdateProductAsync should publish ProductUpdatedMessage with the correct ID when updation is successful")]
    public async Task UpdateProductAsync_ShouldPublishMessage_WhenSuccess()
    {
        // Arrange
        var product = _fixture.Create<UpdateProductDto>();


        // Act
        var result = await _service.UpdateProductAsync(product);


        // Assert
        _updatePublisherMock.Verify(
            p => p.PublishAsync(
                It.Is<ProductUpdatedMessage>(m => m.Id == product.Id),
                default),
            Times.Once());
    }

    [Fact(DisplayName = "UpdateProductAsync should return updated ProductDto when updation is successful")]
    public async Task UpdateProductAsync_ShouldReturnProductDto_WhenSuccess()
    {
        // Arrange
        var product = _fixture.Create<UpdateProductDto>();


        // Act
        var result = await _service.UpdateProductAsync(product);


        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(product, opt =>
            opt.ExcludingMissingMembers());
    }
}
