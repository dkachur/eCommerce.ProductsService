using eCommerce.ProductsService.Domain.Entities;

namespace eCommerce.ProductsService.Application.RepositoryContracts;

public interface IProductsRepository
{
    Task<IEnumerable<Product>> GetProducts();
    Task<IEnumerable<Product>> GetBySearchString(string searchString);
    Task<Product?> AddProduct(Product product);
    Task<Product?> UpdateProduct(Product product);
    Task<bool> DeleteProduct(Guid id);
}
