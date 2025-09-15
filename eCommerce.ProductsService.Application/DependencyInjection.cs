using eCommerce.ProductsService.Application.ServiceContracts;
using eCommerce.ProductsService.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace eCommerce.ProductsService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //TODO : Add services
        services.AddScoped<IProductsService, Services.ProductsService>();
        services.AddValidatorsFromAssemblyContaining<AddProductDtoValidator>();
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en-US");

        return services;
    }
}
