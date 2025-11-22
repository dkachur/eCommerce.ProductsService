using eCommerce.ProductsService.Application.Messaging;
using eCommerce.ProductsService.Infrastructure.Messaging.Interfaces;
using eCommerce.ProductsService.Tests.Integration.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Data;

namespace eCommerce.ProductsService.Tests.Integration.API;

public class ProductsApiFactory : WebApplicationFactory<Program>
{
    private IMessagePublisher<ProductUpdatedMessage>? _updatedPublisher;
    private IMessagePublisher<ProductDeletedMessage>? _deletedPublisher;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        _updatedPublisher = new Mock<IMessagePublisher<ProductUpdatedMessage>>().Object;
        _deletedPublisher = new Mock<IMessagePublisher<ProductDeletedMessage>>().Object;

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IDbConnection>();
            services.AddTransient<IDbConnection>(_ =>
            {
                var conn = new SqliteConnection(TestDatabase.ConnectionString);
                conn.Open();
                return conn;
            });

            services.RemoveAll<IRabbitMqConnectionManager>();
            services.RemoveAll<IRabbitMqPublisher>();
            services.RemoveAll<IMessagePublisher<ProductUpdatedMessage>>();
            services.RemoveAll<IMessagePublisher<ProductDeletedMessage>>();

            services.AddSingleton<IMessagePublisher<ProductUpdatedMessage>>(_ => _updatedPublisher);
            services.AddSingleton<IMessagePublisher<ProductDeletedMessage>>(_ => _deletedPublisher);
        });
    }
}
