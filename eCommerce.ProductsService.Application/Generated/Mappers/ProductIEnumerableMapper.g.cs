using System;
using System.Collections.Generic;
using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Domain.Entities;

namespace System.Collections.Generic
{
    public static partial class ProductIEnumerableMapper
    {
        public static List<ProductDto> AdaptToProductDtoList(this IEnumerable<Product> p1)
        {
            if (p1 == null)
            {
                return null;
            }
            List<ProductDto> result = new List<ProductDto>();
            
            IEnumerator<Product> enumerator = p1.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                Product item = enumerator.Current;
                result.Add(new ProductDto(item.Id, item.Name, Enum.Parse<CategoryOptions>(item.Category, true), item.UnitPrice, item.QuantityInStock));
            }
            return result;
            
        }
    }
}