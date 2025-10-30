using eCommerce.ProductsService.Domain.Entities;

namespace eCommerce.ProductsService.Application.RepositoryContracts;

/// <summary>
/// Represents data access logic for managing <see cref="Product"/> entities.
/// </summary>
public interface IProductsRepository
{
    /// <summary>
    /// Retrieves all products from the storage.
    /// </summary>
    /// <returns>
    /// A collection of all <see cref="Product"/> instances.
    /// If the storage contains no products, an empty collection is returned.
    /// </returns>
    Task<IEnumerable<Product>> GetProductsAsync();

    /// <summary>
    /// Retrieves product with the specified ID.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>
    /// A <see cref="Product"/> with the specified ID if found;
    /// otherwise, <c>null</c>.
    /// </returns>
    Task<Product?> GetProductByIdAsync(Guid id);

    /// <summary>
    /// Retrieves products with the specified IDs from the storage.
    /// </summary>
    /// <param name="ids">The collection of product IDs.</param>
    /// <returns>
    /// A collection of <see cref="Product"/> corresponding to the specified IDs.
    /// If none of the IDs exist, an empty collection is returned.
    /// </returns>
    Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<Guid> ids);

    /// <summary>
    /// Retrieves all products whose <c>Name</c> or <c>Category</c> contains the specified <paramref name="searchString"/>.
    /// </summary>
    /// <param name="searchString">The string value to search for.</param>
    /// <returns>
    /// A collection of <see cref="Product"/> instances that match the search criteria.  
    /// If no products are found, an empty collection is returned.
    /// </returns>
    Task<IEnumerable<Product>> GetBySearchStringAsync(string searchString);

    /// <summary>
    /// Retrieves product ID for each ID from <paramref name="ids"/> if it exists.
    /// </summary>
    /// <param name="ids">The product unique identifiers.</param>
    /// <returns>
    /// A collection of <see cref="Guid"/> with product IDs which exist.
    /// </returns>
    Task<IEnumerable<Guid>> GetExistingProductIdsAsync(IEnumerable<Guid> ids);

    /// <summary>
    /// Adds product to the storage.
    /// </summary>
    /// <param name="product">The product to add.</param>
    /// <returns>
    /// The added <see cref="Product"/> if adding is successful;
    /// otherwise, <c>null</c>.
    /// </returns>
    Task<Product?> AddProductAsync(Product product);

    /// <summary>
    /// Updates product in the storage.
    /// </summary>
    /// <param name="product">The updated product.</param>
    /// <returns>
    /// The updated <see cref="Product"/> if updation is successful;
    /// otherwise, <c>null</c>.
    /// </returns>
    Task<Product?> UpdateProductAsync(Product product);

    /// <summary>
    /// Deletes product with the specified ID from the storage.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>
    /// <c>true</c> if delition is successful;
    /// otherwise, <c>false.</c>
    /// </returns>
    Task<bool> DeleteProductAsync(Guid id);
}
