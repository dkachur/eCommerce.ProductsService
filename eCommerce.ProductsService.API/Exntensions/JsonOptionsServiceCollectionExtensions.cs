using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;

namespace eCommerce.ProductsService.API.Exntensions;

public static class JsonOptionsServiceCollectionExtensions
{
    public static IServiceCollection AddJsonStringEnumConverter(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(opt =>
            opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        return services;
    }
}
