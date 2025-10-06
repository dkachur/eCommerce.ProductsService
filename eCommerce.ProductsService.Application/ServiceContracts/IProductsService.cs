using eCommerce.ProductsService.Application.DTOs;
using FluentResults;

namespace eCommerce.ProductsService.Application.ServiceContracts;

/// <summary>
/// Defines methods for product operations.
/// </summary>
public interface IProductsService
{
    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <returns>
    /// A result containing a list of <see cref="ProductDto"/> if retrieving is successful;
    /// otherwise, a result containing an error.
    /// </returns>
    Task<Result<List<ProductDto>>> GetProductsAsync();

    /// <summary>
    /// Retrieves products that contain <paramref name="searchString"/> in the name or category.
    /// </summary>
    /// <param name="searchString">The search value.</param>
    /// <returns>
    /// A result containing a list of <see cref="ProductDto"/> that contain <paramref name="searchString"/> if search is successful;
    /// otherwise, a result containing an error.
    /// </returns>
    Task<Result<List<ProductDto>>> GetBySearchStringAsync(string searchString);

    /// <summary>
    /// Retrieves product by its ID.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>
    /// A result containing a <see cref="ProductDto"/> with specified ID if retrieving is successful;
    /// otherwise, a result containing an error.
    /// </returns>
    Task<Result<ProductDto>> GetByIdAsync(Guid productId);

    /// <summary>
    /// Checks which of the specified product IDs exist in the storage.
    /// </summary>
    /// <param name="productIds">A collection of product IDs to check.</param>
    /// <returns>
    /// A result containing a dictionary where each key is a product ID 
    /// from <paramref name="productIds"/> and the value is <c>true</c> if the product exists,
    /// or <c>false</c> if it does not.
    /// </returns>
    Task<Result<Dictionary<Guid, bool>>> CheckProductsExistAsync(IEnumerable<Guid> productIds);

    /// <summary>
    /// Adds product.
    /// </summary>
    /// <param name="product">The product to add.</param>
    /// <returns>
    /// A result containig the added <see cref="ProductDto"/> if adding is successful;
    /// otherwise, a result containing an error.
    /// </returns>
    Task<Result<ProductDto>> AddProductAsync(AddProductDto product);

    /// <summary>
    /// Updates product with ID specified in the request.
    /// </summary>
    /// <param name="product">The updated product.</param>
    /// <returns>
    /// A result containig the updated <see cref="ProductDto"/> if updation is successful;
    /// otherwise, a result containing an error.
    /// </returns>
    Task<Result<ProductDto>> UpdateProductAsync(UpdateProductDto product);

    /// <summary>
    /// Deletes product with specified ID.
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete.</param>
    /// <returns>
    /// A result with successful status if deletion is successful;
    /// otherwise, a result containing an error.
    /// </returns>
    Task<Result> DeleteProductAsync(Guid id);
}
