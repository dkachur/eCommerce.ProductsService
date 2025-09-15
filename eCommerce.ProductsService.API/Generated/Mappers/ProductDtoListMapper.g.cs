using System.Collections.Generic;
using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Application.DTOs;

namespace System.Collections.Generic
{
    public static partial class ProductDtoListMapper
    {
        public static List<ProductResponse> AdaptToProductResponseList(this List<ProductDto> p1)
        {
            if (p1 == null)
            {
                return null;
            }
            List<ProductResponse> result = new List<ProductResponse>(p1.Count);
            
            int i = 0;
            int len = p1.Count;
            
            while (i < len)
            {
                ProductDto item = p1[i];
                result.Add(item == null ? null : new ProductResponse(item.Id, item.Name, item.Category, item.UnitPrice, item.QuantityInStock) {});
                i++;
            }
            return result;
            
        }
    }
}