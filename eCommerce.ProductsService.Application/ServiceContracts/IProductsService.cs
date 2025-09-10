using eCommerce.ProductsService.Application.DTOs;
using FluentResults;

namespace eCommerce.ProductsService.Application.ServiceContracts;

public interface IProductsService
{
    Task<Result<List<ProductDto>>> GetProducts();
    Task<Result<List<ProductDto>>> GetBySearchString(string searchString);
    Task<Result<ProductDto>> AddProduct(AddProductDto product);
    Task<Result<ProductDto>> UpdateProduct(UpdateProductDto product);
    Task<Result> DeleteProduct(Guid id);
}
