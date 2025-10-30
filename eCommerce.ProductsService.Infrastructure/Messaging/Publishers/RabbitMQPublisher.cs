using eCommerce.ProductsService.Application.Messaging;
using eCommerce.ProductsService.Infrastructure.Messaging.Interfaces;
using eCommerce.ProductsService.Infrastructure.Messaging.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;

namespace eCommerce.ProductsService.Infrastructure.Messaging.Publishers;

public class RabbitMQPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly IRabbitMqConnectionManager _connectionManager;
    private IChannel? _channel;
    private bool _disposed;
    private readonly string _exchange;

    public RabbitMQPublisher(IRabbitMqConnectionManager connectionManager, IOptions<RabbitMqOptions> options)
    {
        _connectionManager = connectionManager;
        _exchange = options.Value.Exchange;
    }

    public async Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default)
    {
        var body = JsonSerializer.SerializeToUtf8Bytes(message);
        var channel = await GetOrCreateChannelAsync(ct);

        await channel.BasicPublishAsync(
            exchange: _exchange,
            routingKey: routingKey,
            body: body,
            ct);
    }

    private async Task<IChannel> GetOrCreateChannelAsync(CancellationToken ct = default)
    {
        if (_channel is null || _channel.IsClosed)
        {
            _channel = await _connectionManager.CreateChannel();
            await _channel.ExchangeDeclareAsync(
                exchange: _exchange,
                type: ExchangeType.Direct,
                durable: true,
                cancellationToken: ct);
        }

        return _channel;
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        if (_channel is not null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
