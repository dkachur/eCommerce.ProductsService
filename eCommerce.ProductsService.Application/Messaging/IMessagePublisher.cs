namespace eCommerce.ProductsService.Application.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default);
}
