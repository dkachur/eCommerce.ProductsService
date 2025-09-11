using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Domain.Entities;

namespace eCommerce.ProductsService.Application.DTOs
{
    public static partial class AddProductDtoMapper
    {
        public static Product AdaptToProduct(this AddProductDto src)
        {
            return Product.New(src.Name, src.Category.ToString(), src.UnitPrice, src.QuantityInStock);
        }
        public static Product AdaptTo(this AddProductDto src, Product p1)
        {
            return Product.New(src.Name, src.Category.ToString(), src.UnitPrice, src.QuantityInStock);
        }
    }
}