using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Errors;
using eCommerce.ProductsService.Application.Exntensions;
using eCommerce.ProductsService.Application.RepositoryContracts;
using eCommerce.ProductsService.Application.ServiceContracts;
using eCommerce.ProductsService.Domain.Entities;
using FluentResults;
using FluentValidation;

namespace eCommerce.ProductsService.Application.Services;

public class ProductsService : IProductsService
{
    private readonly IProductsRepository _repo;
    private readonly IValidator<AddProductDto> _addProductValidator;
    private readonly IValidator<UpdateProductDto> _updateProductValidator;

    public ProductsService(
        IProductsRepository repo, 
        IValidator<AddProductDto> addProductValidator, 
        IValidator<UpdateProductDto> updateProductValidator)
    {
        _repo = repo;
        _addProductValidator = addProductValidator;
        _updateProductValidator = updateProductValidator;
    }

    public async Task<Result<ProductDto>> AddProductAsync(AddProductDto product)
    {
        var validationResult = await _addProductValidator.ValidateAsync(product);
        if (!validationResult.IsValid)
            return Result.Fail<ProductDto>(validationResult.ToValidationErrors());

        var addedProduct = await _repo.AddProductAsync(product.AdaptToProduct());
        if (addedProduct is null)
            return Result.Fail<ProductDto>(new PersistenceError("Product cannot be saved."));

        return Result.Ok(addedProduct.AdaptToProductDto());
    }

    public async Task<Result> DeleteProductAsync(Guid id)
    {
        bool deleted = await _repo.DeleteProductAsync(id);
        if (!deleted)
            return Result.Fail(ProductNotFoundError.WithId(id));

        return Result.Ok();
    }

    public async Task<Result<ProductDto>> GetByIdAsync(Guid productId)
    {
        var product = await _repo.GetByIdAsync(productId);
        if (product is null)
            return Result.Fail<ProductDto>(ProductNotFoundError.WithId(productId));

        return Result.Ok(product.AdaptToProductDto());
    }

    public async Task<Result<List<ProductDto>>> GetBySearchStringAsync(string searchString)
    {
        var products = string.IsNullOrWhiteSpace(searchString) 
            ? await _repo.GetProductsAsync()
            : await _repo.GetBySearchStringAsync(searchString);

        return Result.Ok(products.AdaptToProductDtoList());
    }

    public async Task<Result<List<ProductDto>>> GetProductsAsync()
    {
        IEnumerable<Product> products = await _repo.GetProductsAsync();
        
        return Result.Ok(products.AdaptToProductDtoList());
    }

    public async Task<Result<ProductDto>> UpdateProductAsync(UpdateProductDto product)
    {
        var validationResult = await _updateProductValidator.ValidateAsync(product);
        if (!validationResult.IsValid)
            return Result.Fail<ProductDto>(validationResult.ToValidationErrors());

        var updatedProduct = await _repo.UpdateProductAsync(product.AdaptToProduct());
        if (updatedProduct is null)
            return Result.Fail<ProductDto>(ProductNotFoundError.WithId(product.Id));

        return Result.Ok(updatedProduct.AdaptToProductDto());
    }
}
