namespace eCommerce.ProductsService.Application.DTOs;

public record ProductNameUpdateMessage(Guid ProductId, string NewName);
