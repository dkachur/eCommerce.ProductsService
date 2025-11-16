namespace eCommerce.ProductsService.Infrastructure.Messaging.Interfaces;

public interface IRabbitMqPublisher : IAsyncDisposable
{
    Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default);
}
