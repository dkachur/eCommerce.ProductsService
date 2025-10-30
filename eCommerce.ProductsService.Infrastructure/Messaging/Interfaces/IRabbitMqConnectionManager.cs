using RabbitMQ.Client;

namespace eCommerce.ProductsService.Infrastructure.Messaging.Interfaces;

public interface IRabbitMqConnectionManager : IAsyncDisposable
{
    IConnection Connection { get; }
    Task InitializeAsync(CancellationToken ct = default);
    Task<IChannel> CreateChannel();
}
