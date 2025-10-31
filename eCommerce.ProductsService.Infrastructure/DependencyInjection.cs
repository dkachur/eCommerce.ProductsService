using eCommerce.ProductsService.Application.Messaging;
using eCommerce.ProductsService.Application.RepositoryContracts;
using eCommerce.ProductsService.Infrastructure.DbContext;
using eCommerce.ProductsService.Infrastructure.Messaging.ConnectionManagers;
using eCommerce.ProductsService.Infrastructure.Messaging.HostedServices;
using eCommerce.ProductsService.Infrastructure.Messaging.Interfaces;
using eCommerce.ProductsService.Infrastructure.Messaging.Options;
using eCommerce.ProductsService.Infrastructure.Messaging.Publishers;
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

        services.AddMySQL(config);
        services.AddRabbitMQ(config);

        return services;
    }

    private static IServiceCollection AddMySQL(this IServiceCollection services, IConfiguration config)
    {
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

    private static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration config)
    {
        var rabbitOptions = new RabbitMqOptions
        {
            Host = config["RABBITMQ_HOST"] ?? throw new InvalidOperationException("Missing env: RABBITMQ_HOST"),
            Port = int.TryParse(config["RABBITMQ_PORT"], out var port) ? port : 5672,
            Username = config["RABBITMQ_USER"] ?? "guest",
            Password = config["RABBITMQ_PASS"] ?? "guest",
            ProductsExchange = config["RABBITMQ_PRODUCTS_EXCHANGE"] ?? "products.exchange",
            ProductNameUpdatedRoutingKey = config["RABBITMQ_PRODUCT_NAME_UPDATED_ROUTING_KEY"] ?? "product.name.updated",
        };

        services.Configure<RabbitMqOptions>(opt =>
        {
            opt.Host = rabbitOptions.Host;
            opt.Port = rabbitOptions.Port;
            opt.Username = rabbitOptions.Username;
            opt.Password = rabbitOptions.Password;
            opt.ProductsExchange = rabbitOptions.ProductsExchange;
            opt.ProductNameUpdatedRoutingKey = rabbitOptions.ProductNameUpdatedRoutingKey;
        });

        services.AddSingleton<IRabbitMqConnectionManager, RabbitMqConnectionManager>();

        services.AddHostedService<RabbitMqConnectionHostedService>();

        services.AddSingleton<RabbitMqPublisher>();
        services.AddSingleton<IMessagePublisher<ProductNameUpdatedMessage>, ProductNameUpdatedPublisher>();

        return services;
    }
}
