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
        services.AddScoped<IProductsRepository, ProductsRepository>();
        
        string connectionStringTemplate = config.GetConnectionString("Default")!;
        string connectionString = connectionStringTemplate
            .Replace("${MYSQL_HOST}", config["MYSQL_HOST"])
            .Replace("${MYSQL_PORT}", config["MYSQL_PORT"])
            .Replace("${MYSQL_DB}", config["MYSQL_DB"])
            .Replace("${MYSQL_USER}", config["MYSQL_USER"])
            .Replace("${MYSQL_PASSWORD}", config["MYSQL_PASSWORD"]);

        services.AddScoped<IDbConnection>(_ =>
            new MySqlConnection(connectionString));

        services.AddTransient<DapperDbContext>();

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services;
    }
}
