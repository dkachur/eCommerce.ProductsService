using eCommerce.ProductsService.Application.Errors;
using eCommerce.ProductsService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace eCommerce.ProductsService.Tests.Application.Services;

public partial class ProductsServiceTests
{
    [Fact(DisplayName = "GetByIdAsync should return ProductNotFoundError when product does not exist")]
    public async Task GetByIdAsync_ShouldReturnProductNotFoundError_WhenProductDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetProductByIdAsync(id))
            .ReturnsAsync(null as Product);


        // Act
        var result = await _service.GetByIdAsync(id);


        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ProductNotFoundError>();
    }

    [Fact(DisplayName = "GetByIdAsync should return product when exists")]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();


        // Act
        var result = await _service.GetByIdAsync(id);


        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}
