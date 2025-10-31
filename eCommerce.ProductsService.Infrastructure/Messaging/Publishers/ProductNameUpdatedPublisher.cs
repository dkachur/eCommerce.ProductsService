using eCommerce.ProductsService.Application.Messaging;
using eCommerce.ProductsService.Infrastructure.Messaging.Options;
using Microsoft.Extensions.Options;

namespace eCommerce.ProductsService.Infrastructure.Messaging.Publishers;

public class ProductNameUpdatedPublisher : IMessagePublisher<ProductNameUpdatedMessage>
{
    private readonly RabbitMqPublisher _publisher;
    private readonly string _routingKey;

    public ProductNameUpdatedPublisher(RabbitMqPublisher publisher, IOptions<RabbitMqOptions> options)
    {
        _publisher = publisher;
        _routingKey = options.Value.ProductNameUpdatedRoutingKey;
    }

    public async Task PublishAsync(ProductNameUpdatedMessage message, CancellationToken ct = default)
    {
        await _publisher.PublishAsync(message, _routingKey, ct);
    }
}
