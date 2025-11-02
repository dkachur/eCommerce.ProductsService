using eCommerce.ProductsService.Application.Messaging;
using eCommerce.ProductsService.Infrastructure.Messaging.Options;
using Microsoft.Extensions.Options;

namespace eCommerce.ProductsService.Infrastructure.Messaging.Publishers;

public class ProductUpdatedPublisher : IMessagePublisher<ProductUpdatedMessage>
{
    private readonly RabbitMqPublisher _publisher;
    private readonly string _routingKey;

    public ProductUpdatedPublisher(RabbitMqPublisher publisher, IOptions<RabbitMqOptions> options)
    {
        _publisher = publisher;
        _routingKey = options.Value.ProductUpdatedRoutingKey;
    }

    public async Task PublishAsync(ProductUpdatedMessage message, CancellationToken ct = default)
    {
        await _publisher.PublishAsync(message, _routingKey, ct);
    }
}
