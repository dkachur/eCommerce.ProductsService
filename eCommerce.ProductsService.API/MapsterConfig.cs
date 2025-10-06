using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Enums;
using Mapster;

namespace eCommerce.ProductsService.API;

public class MapsterConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddProductRequest, AddProductDto>()
            .MapWith(src => new AddProductDto(
                src.Name,
                Enum.Parse<CategoryOptions>(src.Category, true),
                src.UnitPrice,
                src.QuantityInStock));

        config.NewConfig<List<ProductDto>, List<ProductResponse>>()
            .GenerateMapper(MapType.Map);

        config.NewConfig<UpdateProductRequest, UpdateProductDto>()
            .MapWith(src => new UpdateProductDto(
                src.Id,
                src.Name,
                Enum.Parse<CategoryOptions>(src.Category, true),
                src.UnitPrice,
                src.QuantityInStock));

        config.NewConfig<Dictionary<Guid, bool>, List<ProductExistResponse>>()
            .MapWith(src => src
                .Select(kvp => new ProductExistResponse(kvp.Key, kvp.Value))
                .ToList()
            )
            .GenerateMapper(MapType.Map);
    }
}
