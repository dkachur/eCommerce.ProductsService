using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace eCommerce.ProductsService.Tests.Integration.Infrastructure.Common;

public class TestDatabase : IDisposable
{
    public IDbConnection Connection { get; }

    public TestDatabase()
    {
        var connection = new SqliteConnection("Data source=:memory:");
        connection.Open();

        Connection = connection;

        SqlMapper.AddTypeHandler(new GuidTypeHandler());

        CreateSchema();
    }

    private void CreateSchema()
    {
        var cmd = Connection.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE products (
                id TEXT PRIMARY KEY,
                name TEXT NOT NULL,
                category TEXT NOT NULL,
                unit_price REAL NOT NULL,
                quantity_in_stock INTEGER NOT NULL
                );
            """;
        cmd.ExecuteNonQuery();
    }

    public void Dispose()
    {
        Connection?.Dispose();
        GC.SuppressFinalize(this);
    }
}
