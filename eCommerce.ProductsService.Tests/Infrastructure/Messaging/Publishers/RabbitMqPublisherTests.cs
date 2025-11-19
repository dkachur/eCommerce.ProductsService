using Bogus;
using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Application.Messaging;
using eCommerce.ProductsService.Infrastructure.Messaging.Interfaces;
using eCommerce.ProductsService.Infrastructure.Messaging.Options;
using eCommerce.ProductsService.Infrastructure.Messaging.Publishers;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using RabbitMQ.Client;
using System.Text.Json;

namespace eCommerce.ProductsService.Tests.Infrastructure.Messaging.Publishers;

public class RabbitMqPublisherTests
{
    private readonly RabbitMqPublisher _publisher;

    private readonly Mock<IRabbitMqConnectionManager> _connectionManagerMock;
    private readonly Faker _faker;

    private const string Exchange = "products.exchange";
    private const string RoutingKey = "product.updated";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public RabbitMqPublisherTests()
    {
        _faker = new Faker();
        _connectionManagerMock = new();
        var options = Options.Create<RabbitMqOptions>(new() { ProductsExchange = Exchange });

        _publisher = new(_connectionManagerMock.Object, options);
    }

    [Fact]
    public async Task PublishAsync_ShouldPublishCorrectMessage_WhenCalled()
    {
        // Arrange
        var channelMock = new Mock<IChannel>();

        _connectionManagerMock
            .Setup(c => c.CreateChannel())
            .ReturnsAsync(channelMock.Object);

        var message = new ProductUpdatedMessage(
            _faker.Random.Guid(),
            _faker.Commerce.ProductName(),
            _faker.Random.Enum<CategoryOptions>().ToString(),
            _faker.Random.Double(1, 100),
            _faker.Random.Int(0, 100));

        var body = JsonSerializer.SerializeToUtf8Bytes(message, JsonOptions);


        // Act
        await _publisher.PublishAsync(message, RoutingKey);


        // Assert
        var invocs = channelMock.Invocations
            .Where(i => i.Method.Name == nameof(channelMock.Object.BasicPublishAsync))
            .ToList();

        invocs.Count.Should().Be(1);

        var invoc = invocs.First();

        // Body with message is 4th argument
        invoc.Arguments[4].Should().BeOfType<ReadOnlyMemory<byte>>();
        var invocBody = ((ReadOnlyMemory<byte>)invoc.Arguments[4]).ToArray();

        // Exchange is argument at position 0
        var invocExchange = invoc.Arguments[0] as string;

        // Routing key is argument at position 1
        var invocRoutingKey = invoc.Arguments[1] as string;

        invocBody.Should().BeEquivalentTo(body);
        invocExchange.Should().Be(Exchange);
        invocRoutingKey.Should().Be(RoutingKey);
    }
}
