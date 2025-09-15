using System;
using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Enums;

namespace eCommerce.ProductsService.API.DTOs
{
    public static partial class UpdateProductRequestMapper
    {
        public static UpdateProductDto AdaptToUpdateProductDto(this UpdateProductRequest src)
        {
            return new UpdateProductDto(src.Id, src.Name, Enum.Parse<CategoryOptions>(src.Category, true), src.UnitPrice, src.QuantityInStock);
        }
        public static UpdateProductDto AdaptTo(this UpdateProductRequest src, UpdateProductDto p1)
        {
            return new UpdateProductDto(src.Id, src.Name, Enum.Parse<CategoryOptions>(src.Category, true), src.UnitPrice, src.QuantityInStock);
        }
    }
}