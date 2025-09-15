namespace eCommerce.ProductsService.API.Exntensions;

public static class CorsServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguredCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policyBuilder =>
                policyBuilder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                );
        });
           
        return services;
    }
}
