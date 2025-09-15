using System.Data;

namespace eCommerce.ProductsService.Infrastructure.DbContext;

public class DapperDbContext
{
    private readonly IDbConnection _dbConnection;
    public IDbConnection DbConnection => _dbConnection;

    public DapperDbContext(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
}
