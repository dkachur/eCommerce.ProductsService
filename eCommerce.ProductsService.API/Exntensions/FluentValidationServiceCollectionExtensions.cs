using FluentValidation;

namespace eCommerce.ProductsService.API.Exntensions;

public static class FluentValidationServiceCollectionExtensions
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>();

        return services;
    }
}
