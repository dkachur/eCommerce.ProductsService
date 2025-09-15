namespace eCommerce.ProductsService.API.Exntensions;

public static class SwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options => options.IncludeXmlComments("api.xml"));

        return services;
    }
}
