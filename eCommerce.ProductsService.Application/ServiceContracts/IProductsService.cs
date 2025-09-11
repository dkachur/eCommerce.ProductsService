using eCommerce.ProductsService.Application.DTOs;
using FluentResults;

namespace eCommerce.ProductsService.Application.ServiceContracts;

public interface IProductsService
{
    Task<Result<List<ProductDto>>> GetProductsAsync();
    Task<Result<List<ProductDto>>> GetBySearchStringAsync(string searchString);
    Task<Result<ProductDto>> GetByIdAsync(Guid productId);
    Task<Result<ProductDto>> AddProductAsync(AddProductDto product);
    Task<Result<ProductDto>> UpdateProductAsync(UpdateProductDto product);
    Task<Result> DeleteProductAsync(Guid id);
}
