namespace eCommerce.ProductsService.Application.Messaging;

public interface IMessagePublisher<T>
{
    Task PublishAsync(T message, CancellationToken ct = default);
}
