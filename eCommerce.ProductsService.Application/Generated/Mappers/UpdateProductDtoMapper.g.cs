using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Domain.Entities;

namespace eCommerce.ProductsService.Application.DTOs
{
    public static partial class UpdateProductDtoMapper
    {
        public static Product AdaptToProduct(this UpdateProductDto src)
        {
            return Product.Restore(src.Id, src.Name, src.Category.ToString(), src.UnitPrice, src.QuantityInStock);
        }
        public static Product AdaptTo(this UpdateProductDto src, Product p1)
        {
            return Product.Restore(src.Id, src.Name, src.Category.ToString(), src.UnitPrice, src.QuantityInStock);
        }
    }
}