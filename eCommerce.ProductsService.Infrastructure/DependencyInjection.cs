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

        string connectionStringTemplate = config.GetConnectionString("Default")!;
        string connectionString = connectionStringTemplate
            .Replace("${MYSQL_HOST}", Environment.GetEnvironmentVariable("MYSQL_HOST"))
            .Replace("${MYSQL_PORT}", Environment.GetEnvironmentVariable("MYSQL_PORT"))
            .Replace("${MYSQL_DB}", Environment.GetEnvironmentVariable("MYSQL_DB"))
            .Replace("${MYSQL_USER}", Environment.GetEnvironmentVariable("MYSQL_USER"))
            .Replace("${MYSQL_PASSWORD}", Environment.GetEnvironmentVariable("MYSQL_PASSWORD"));

        services.AddScoped<IDbConnection>(_ =>
            new MySqlConnection(connectionString));

        services.AddTransient<DapperDbContext>();

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services;
    }
}
