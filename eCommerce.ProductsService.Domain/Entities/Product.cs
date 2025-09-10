namespace eCommerce.ProductsService.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } 
    public string Category { get; private set; }
    public double UnitPrice { get; private set; }
    public int QuantityInStock { get; private set; }

    public static Product New(string name, string category, double unitPrice, int quantityInStock)
        => new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Category = category,
            UnitPrice = unitPrice,
            QuantityInStock = quantityInStock
        };

    public static Product Restore(Guid id, string name, string category, double unitPrice, int quantityInStock)
        => new()
        {
            Id = id,
            Name = name,
            Category = category,
            UnitPrice = unitPrice,
            QuantityInStock = quantityInStock
        };

    private Product() { }
}
