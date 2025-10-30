using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Errors;
using eCommerce.ProductsService.Application.Exntensions;
using eCommerce.ProductsService.Application.Messaging;
using eCommerce.ProductsService.Application.RepositoryContracts;
using eCommerce.ProductsService.Application.ServiceContracts;
using eCommerce.ProductsService.Domain.Entities;
using FluentResults;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace eCommerce.ProductsService.Application.Services;

public class ProductsService : IProductsService
{
    private readonly IProductsRepository _repo;
    private readonly IValidator<AddProductDto> _addProductValidator;
    private readonly IValidator<UpdateProductDto> _updateProductValidator;
    private readonly ILogger<ProductsService> _logger;
    private readonly IMessagePublisher _messagePublisher;
    private const string UpdateNameRoutingKey = "product.update.name";

    public ProductsService(
        IProductsRepository repo,
        IValidator<AddProductDto> addProductValidator,
        IValidator<UpdateProductDto> updateProductValidator,
        ILogger<ProductsService> logger,
        IMessagePublisher messagePublisher)
    {
        _repo = repo;
        _addProductValidator = addProductValidator;
        _updateProductValidator = updateProductValidator;
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task<Result<ProductDto>> AddProductAsync(AddProductDto product)
    {
        var validationResult = await _addProductValidator.ValidateAsync(product);
        if (!validationResult.IsValid)
            return Result.Fail<ProductDto>(validationResult.ToValidationErrors());

        var addedProduct = await _repo.AddProductAsync(product.AdaptToProduct());
        if (addedProduct is null)
        {
            _logger.LogError("Product with name {ProductName} was not saved", product.Name);
            return Result.Fail<ProductDto>(new PersistenceError("Product cannot be saved."));
        }

        _logger.LogInformation("Product {ProductName} successfully added with ID {ProductId}", addedProduct.Name, addedProduct.Id);
        return Result.Ok(addedProduct.AdaptToProductDto());
    }

    public async Task<Result<Dictionary<Guid, bool>>> CheckProductsExistAsync(IEnumerable<Guid> productIds)
    {
        var distinctIds = productIds.Distinct();

        var existingIds = await _repo.GetExistingProductIdsAsync(distinctIds);
        var existingSet = existingIds.ToHashSet();

        var idExistingInfo = distinctIds.ToDictionary(
            pId => pId,
            pId => existingSet.Contains(pId));

        return Result.Ok(idExistingInfo);
    }

    public async Task<Result> DeleteProductAsync(Guid id)
    {
        bool deleted = await _repo.DeleteProductAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", id);
            return Result.Fail(ProductNotFoundError.WithId(id));
        }

        _logger.LogInformation("Product with ID {ProductId} successfully deleted", id);
        return Result.Ok();
    }

    public async Task<Result<ProductDto>> GetByIdAsync(Guid productId)
    {
        var product = await _repo.GetProductByIdAsync(productId);
        if (product is null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", productId);
            return Result.Fail<ProductDto>(ProductNotFoundError.WithId(productId));
        }    

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

    public async Task<Result<List<ProductDto>>> GetProductsByIdsAsync(IEnumerable<Guid> productIds)
    {
        IEnumerable<Product> products = await _repo.GetProductsByIdsAsync(productIds);

        return Result.Ok(products.AdaptToProductDtoList());
    }

    public async Task<Result<ProductDto>> UpdateProductAsync(UpdateProductDto product)
    {
        var validationResult = await _updateProductValidator.ValidateAsync(product);
        if (!validationResult.IsValid)
            return Result.Fail<ProductDto>(validationResult.ToValidationErrors());

        var existingProduct = await _repo.GetProductByIdAsync(product.Id);
        if (existingProduct is null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", product.Id);
            return Result.Fail<ProductDto>(ProductNotFoundError.WithId(product.Id));
        }

        var updatedProduct = await _repo.UpdateProductAsync(product.AdaptToProduct());
        if (updatedProduct is null)
        {
            _logger.LogWarning("Product with ID {ProductId} was not updated", product.Id);
            return Result.Fail<ProductDto>(new PersistenceError($"Product with ID {product.Id} was not updated."));
        }

        var isNameChanged = existingProduct.Name != updatedProduct.Name;
        if (isNameChanged)
        {
            var message = new ProductNameUpdateMessage(updatedProduct.Id, updatedProduct.Name);
            await _messagePublisher.PublishAsync(message, UpdateNameRoutingKey);
        }

        _logger.LogInformation("Product with ID {ProductId} successfully updated", product.Id);
        return Result.Ok(updatedProduct.AdaptToProductDto());
    }
}
