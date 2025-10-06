using Dapper;
using eCommerce.ProductsService.Application.RepositoryContracts;
using eCommerce.ProductsService.Domain.Entities;
using eCommerce.ProductsService.Infrastructure.DbContext;

namespace eCommerce.ProductsService.Infrastructure.Repositories;

public class ProductsRepository : IProductsRepository
{
    private readonly DapperDbContext _dbContext;

    public ProductsRepository(DapperDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> AddProductAsync(Product product)
    {
        string query = """
            INSERT INTO products
            (id, name, category, unit_price, quantity_in_stock)
            VALUES(@Id, @Name, @Category, @UnitPrice, @QuantityInStock);
            """;

        int rowsAffected = await _dbContext.DbConnection.ExecuteAsync(query, product);
        return rowsAffected > 0 ? product : null;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        string query = """
            DELETE FROM products
            WHERE id = @Id;
            """;

        int rowsAffected = await _dbContext.DbConnection.ExecuteAsync(query, new { Id = id });
        return rowsAffected > 0;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        string query = """
            SELECT id, name, category, unit_price, quantity_in_stock 
            FROM products
            WHERE id = @Id;
            """;

        var product = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<Product>(query, new { Id = id });
        return product;
    }

    public async Task<IEnumerable<Product>> GetBySearchStringAsync(string searchString)
    {
        string query = """
            SELECT id, name, category, unit_price, quantity_in_stock 
            FROM products
            WHERE name LIKE @Search OR category LIKE @Search;
            """;

        var products = await _dbContext.DbConnection.QueryAsync<Product>(query, new { Search = $"%{searchString}%" });
        return products;
    }

    public async Task<IEnumerable<Guid>> GetExistingProductIdsAsync(IEnumerable<Guid> ids)
    {
        string query = """
            SELECT id
            FROM products
            WHERE id IN @Ids
            """;

        var existingIds = await _dbContext.DbConnection.QueryAsync<Guid>(query, new { Ids = ids });
        return existingIds;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        string query = """
            SELECT id, name, category, unit_price, quantity_in_stock
            FROM products;
            """;

        var products = await _dbContext.DbConnection.QueryAsync<Product>(query);
        return products;
    }

    public async Task<Product?> UpdateProductAsync(Product product)
    {
        string query = """
            UPDATE products
            SET name = @Name, category = @Category, unit_price = @UnitPrice, quantity_in_stock = @QuantityInStock
            WHERE id = @Id;
            """;

        int rowsAffected = await _dbContext.DbConnection.ExecuteAsync(query, product);
        return rowsAffected > 0 ? product : null;
    }
}
