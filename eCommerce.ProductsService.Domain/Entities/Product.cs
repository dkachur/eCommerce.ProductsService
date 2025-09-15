namespace eCommerce.ProductsService.Domain.Entities;

/// <summary>
/// The product entity class.
/// </summary>
public class Product
{
    /// <summary>
    /// The unique identifier of the product.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// The name of the product.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The category to which the product belongs.
    /// </summary>
    public string Category { get; private set; }

    /// <summary>
    /// The price of a single product unit.
    /// </summary>
    public double UnitPrice { get; private set; }

    /// <summary>
    /// The number of items available in stock.
    /// </summary>
    public int QuantityInStock { get; private set; }


    /// <summary>
    /// Creates new product entity with newly generated ID.
    /// </summary>
    /// <param name="name">The name of the product.</param>
    /// <param name="category">The category to which the product belongs.</param>
    /// <param name="unitPrice">The price of a single product unit.</param>
    /// <param name="quantityInStock">The number of items available in stock.</param>
    /// <returns></returns>
    public static Product New(string name, string category, double unitPrice, int quantityInStock)
        => new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Category = category,
            UnitPrice = unitPrice,
            QuantityInStock = quantityInStock
        };

    /// <summary>
    /// Reconstructs a user with the specified details.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="name">The name of the product.</param>
    /// <param name="category">The category to which the product belongs.</param>
    /// <param name="unitPrice">The price of a single product unit.</param>
    /// <param name="quantityInStock">The number of items available in stock.</param>
    /// <returns></returns>
    public static Product Restore(Guid id, string name, string category, double unitPrice, int quantityInStock)
        => new()
        {
            Id = id,
            Name = name,
            Category = category,
            UnitPrice = unitPrice,
            QuantityInStock = quantityInStock
        };

    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// Use factory methods to create instances of <see cref="Product"/>.
    /// </summary>
    private Product() { }
}
