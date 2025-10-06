using System;
using System.Collections.Generic;
using System.Linq;
using eCommerce.ProductsService.API.DTOs;

namespace System.Collections.Generic
{
    public static partial class GuidBoolDictionaryMapper
    {
        public static List<ProductExistResponse> AdaptToProductExistResponseList(this Dictionary<Guid, bool> src)
        {
            return src.Select<KeyValuePair<Guid, bool>, ProductExistResponse>(funcMain1).ToList<ProductExistResponse>();
        }
        
        private static ProductExistResponse funcMain1(KeyValuePair<Guid, bool> kvp)
        {
            return new ProductExistResponse(kvp.Key, kvp.Value);
        }
    }
}