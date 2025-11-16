using Bogus;
using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Application.Messaging;
using eCommerce.ProductsService.Infrastructure.Messaging.Interfaces;
using eCommerce.ProductsService.Infrastructure.Messaging.Options;
using eCommerce.ProductsService.Infrastructure.Messaging.Publishers;
using Microsoft.Extensions.Options;
using Moq;

namespace eCommerce.ProductsService.Tests.Infrastructure.Messaging.Publishers;

public class ProductUpdatedPublisherTests
{
    private readonly IMessagePublisher<ProductUpdatedMessage> _publisher;

    private readonly Mock<IRabbitMqPublisher> _mqPublisherMock;
    private readonly Faker _faker;

    private const string RoutingKey = "product.updated";

    public ProductUpdatedPublisherTests()
    {
        _faker = new Faker();
        var options = Options.Create(new RabbitMqOptions() { ProductUpdatedRoutingKey = "product.updated" });
        _mqPublisherMock = new Mock<IRabbitMqPublisher>();

        _publisher = new ProductUpdatedPublisher(_mqPublisherMock.Object, options);
    }

    [Fact]
    public async Task PublishAsync_ShouldPublishMessage_WhenCalled()
    {
        // Arrange
        var message = new ProductUpdatedMessage(
            _faker.Random.Guid(),
            _faker.Commerce.ProductName(),
            _faker.Random.Enum<CategoryOptions>().ToString(),
            _faker.Random.Double(1, 100),
            _faker.Random.Int(0, 100));


        // Act
        await _publisher.PublishAsync(message);


        // Assert
        _mqPublisherMock.Verify(p => p.PublishAsync(message, RoutingKey, It.IsAny<CancellationToken>()), Times.Once);
    }
}
