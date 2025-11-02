using eCommerce.ProductsService.Application.Messaging;
using eCommerce.ProductsService.Domain.Entities;

namespace eCommerce.ProductsService.Application.Messaging
{
    public static partial class ProductUpdatedMessageMapper
    {
        public static ProductUpdatedMessage AdaptToProductUpdatedMessage(this Product p1)
        {
            return p1 == null ? null : new ProductUpdatedMessage(p1.Id, p1.Name, p1.Category, p1.UnitPrice, p1.QuantityInStock) {};
        }
        public static ProductUpdatedMessage AdaptTo(this Product p2, ProductUpdatedMessage p3)
        {
            if (p2 == null)
            {
                return null;
            }
            ProductUpdatedMessage result = new ProductUpdatedMessage(p2.Id, p2.Name, p2.Category, p2.UnitPrice, p2.QuantityInStock) {};
            return result;
            
        }
    }
}