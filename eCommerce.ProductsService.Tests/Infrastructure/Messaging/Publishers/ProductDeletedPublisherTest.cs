using Bogus;
using eCommerce.ProductsService.Application.Messaging;
using eCommerce.ProductsService.Infrastructure.Messaging.Interfaces;
using eCommerce.ProductsService.Infrastructure.Messaging.Options;
using eCommerce.ProductsService.Infrastructure.Messaging.Publishers;
using Microsoft.Extensions.Options;
using Moq;

namespace eCommerce.ProductsService.Tests.Infrastructure.Messaging.Publishers;

public class ProductDeletedPublisherTest
{
    private readonly IMessagePublisher<ProductDeletedMessage> _publisher;

    private readonly Mock<IRabbitMqPublisher> _mqPublisherMock;
    private readonly Faker _faker;

    private const string RoutingKey = "product.deleted";

    public ProductDeletedPublisherTest()
    {
        _faker = new Faker();
        var options = Options.Create(new RabbitMqOptions() { ProductDeletedRoutingKey = RoutingKey });
        _mqPublisherMock = new Mock<IRabbitMqPublisher>();

        _publisher = new ProductDeletedPublisher(_mqPublisherMock.Object, options);
    }

    [Fact]
    public async Task PublishAsync_ShouldPublishMessage_WhenCalled()
    {
        // Arrange
        var message = new ProductDeletedMessage(_faker.Random.Guid());


        // Act
        await _publisher.PublishAsync(message);


        // Assert
        _mqPublisherMock.Verify(p => p.PublishAsync(message, RoutingKey, It.IsAny<CancellationToken>()), Times.Once);
    }
}
