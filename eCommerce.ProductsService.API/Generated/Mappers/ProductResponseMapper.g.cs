using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Application.DTOs;

namespace eCommerce.ProductsService.API.DTOs
{
    public static partial class ProductResponseMapper
    {
        public static ProductResponse AdaptToProductResponse(this ProductDto p1)
        {
            return p1 == null ? null : new ProductResponse(p1.Id, p1.Name, p1.Category, p1.UnitPrice, p1.QuantityInStock) {};
        }
        public static ProductResponse AdaptTo(this ProductDto p2, ProductResponse p3)
        {
            if (p2 == null)
            {
                return null;
            }
            ProductResponse result = new ProductResponse(p2.Id, p2.Name, p2.Category, p2.UnitPrice, p2.QuantityInStock) {};
            return result;
            
        }
    }
}