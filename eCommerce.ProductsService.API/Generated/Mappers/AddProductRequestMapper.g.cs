using System;
using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Enums;

namespace eCommerce.ProductsService.API.DTOs
{
    public static partial class AddProductRequestMapper
    {
        public static AddProductDto AdaptToAddProductDto(this AddProductRequest src)
        {
            return new AddProductDto(src.Name, Enum.Parse<CategoryOptions>(src.Category, true), src.UnitPrice, src.QuantityInStock);
        }
        public static AddProductDto AdaptTo(this AddProductRequest src, AddProductDto p1)
        {
            return new AddProductDto(src.Name, Enum.Parse<CategoryOptions>(src.Category, true), src.UnitPrice, src.QuantityInStock);
        }
    }
}