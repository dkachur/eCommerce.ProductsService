using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Domain.Entities;
using Mapster;

namespace eCommerce.ProductsService.Application;

public class MapsterConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddProductDto, Product>()
            .MapWith(src => Product.New(
                src.Name,
                src.Category.ToString(),
                src.UnitPrice,
                src.QuantityInStock));

        config.NewConfig<Product, ProductDto>()
            .MapWith(src => new ProductDto(
                src.Id,
                src.Name,
                Enum.Parse<CategoryOptions>(src.Category, true),
                src.UnitPrice,
                src.QuantityInStock));

        config.NewConfig<UpdateProductDto, Product>()
            .MapWith(src => Product.Restore(
                src.Id,
                src.Name,
                src.Category.ToString(),
                src.UnitPrice,
                src.QuantityInStock));

        config.NewConfig<IEnumerable<Product>, List<ProductDto>>()
            .GenerateMapper(MapType.Map);
    }
}
