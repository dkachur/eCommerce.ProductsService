using System;
using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Domain.Entities;

namespace eCommerce.ProductsService.Application.DTOs
{
    public static partial class ProductDtoMapper
    {
        public static ProductDto AdaptToProductDto(this Product src)
        {
            return new ProductDto(src.Id, src.Name, Enum.Parse<CategoryOptions>(src.Category, true), src.UnitPrice, src.QuantityInStock);
        }
        public static ProductDto AdaptTo(this Product src, ProductDto p1)
        {
            return new ProductDto(src.Id, src.Name, Enum.Parse<CategoryOptions>(src.Category, true), src.UnitPrice, src.QuantityInStock);
        }
    }
}