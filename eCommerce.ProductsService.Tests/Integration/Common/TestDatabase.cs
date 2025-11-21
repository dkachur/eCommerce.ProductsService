using Dapper;
using Microsoft.Data.Sqlite;

namespace eCommerce.ProductsService.Tests.Integration.Common;

public class TestDatabase : IDisposable
{
    private readonly SqliteConnection _connection;

    public SqliteConnection Connection { 
        get 
        {
            if (_connection.State == System.Data.ConnectionState.Open)
                return _connection;

            _connection.Open();
            return _connection;
        } 
        private set { } }

    public const string ConnectionString = "Data Source=file:memdb1?mode=memory&cache=shared";

    public TestDatabase()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        _connection = connection;

        SqlMapper.AddTypeHandler(new GuidTypeHandler());

        CreateSchema();
    }

    public async Task ClearDb()
    {
        if (Connection.State is not System.Data.ConnectionState.Open)
            Connection.Open();

        var cmd = Connection.CreateCommand();
        cmd.CommandText = "DELETE FROM products;";
        await cmd.ExecuteNonQueryAsync();
    }

    private void CreateSchema()
    {
        var cmd = Connection.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS products (
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
