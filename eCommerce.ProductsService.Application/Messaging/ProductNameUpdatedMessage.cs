namespace eCommerce.ProductsService.Application.Messaging;

public record ProductNameUpdatedMessage(Guid ProductId, string NewName);
