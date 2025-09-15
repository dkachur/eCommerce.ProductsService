using eCommerce.ProductsService.Application.RepositoryContracts;
using eCommerce.ProductsService.Infrastructure.DbContext;
using eCommerce.ProductsService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using System.Data;

namespace eCommerce.ProductsService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        //TODO : Add services
        services.AddScoped<IProductsRepository, ProductsRepository>();

        services.AddScoped<IDbConnection>(_ =>
            new MySqlConnection(config.GetConnectionString("MySQL")));
        services.AddTransient<DapperDbContext>();

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services;
    }
}
