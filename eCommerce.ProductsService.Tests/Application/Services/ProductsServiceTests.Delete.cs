using eCommerce.ProductsService.Application.Errors;
using eCommerce.ProductsService.Application.Messaging;
using FluentAssertions;
using Moq;

namespace eCommerce.ProductsService.Tests.Application.Services;

public partial class ProductsServiceTests
{
    [Fact(DisplayName = "DeleteProductAsync should return ProductNotFoundError when product does not exist")]
    public async Task DeleteProductAsync_ShouldReturnProductNotFoundError_WhenProductDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.DeleteProductAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act 
        var result = await _service.DeleteProductAsync(id);


        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ProductNotFoundError>();
    }

    [Fact(DisplayName = "DeleteProductAsync should publish ProductDeletedMessage with the correct ID when deletion is successful")]
    public async Task DeleteProductAsync_ShouldPublishMessage_WhenSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();


        // Act 
        var result = await _service.DeleteProductAsync(id);


        // Assert
        _deletePublisherMock.Verify(
            p => p.PublishAsync(
                It.Is<ProductDeletedMessage>(m => m.ProductId == id), 
                default),
            Times.Once());
    }

    [Fact(DisplayName = "DeleteProductAsync should return successful result when deletion is successful")]
    public async Task DeleteProductAsync_ShouldReturnSuccessfulResult_WhenSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();


        // Act 
        var result = await _service.DeleteProductAsync(id);


        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
