using System.Text.Json;
using System.Text.Json.Serialization;

namespace eCommerce.ProductsService.Tests.Integration.Common;

public static class JsonConfig
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };
}
