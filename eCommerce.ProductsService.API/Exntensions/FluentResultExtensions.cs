using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Errors;
using FluentResults;

namespace eCommerce.ProductsService.API.Exntensions;

public static class FluentResultExtensions
{
    public static IResult ToApiResult<T>(this Result<T> fluentResult, Func<object?, IResult> successFactory)
    {
        if (fluentResult.IsFailed)
            return fluentResult.ToResult().ToErrorResult();

        if (fluentResult.Value is ProductDto dto)
            return successFactory(dto.AdaptToProductResponse());

        if (fluentResult.Value is List<ProductDto> dtos)
            return successFactory(dtos.AdaptToProductResponseList());

        if (fluentResult.Value is Dictionary<Guid, bool> idExistingInfo)
            return successFactory(idExistingInfo.AdaptToProductExistResponseList());

        return successFactory(fluentResult.Value);
    }

    public static IResult ToNoContentApiResult(this Result fluentResult)
    {
        if (fluentResult.IsFailed)
            return fluentResult.ToErrorResult();

        return Results.NoContent();
    }

    public static IResult ToOkApiResult<T>(this Result<T> fluentResult)
        => fluentResult.ToApiResult(Results.Ok);

    public static IResult ToCreatedProductApiResult(this Result<ProductDto> fluentResult, string baseLocation)
        => fluentResult.ToApiResult(r => {
            string location = $"{baseLocation}{fluentResult.Value.Id}";
            return Results.Created(location, r);
        });

    private static IResult ToErrorResult(this Result fluentResult)
    {
        var firstError = fluentResult.Errors.FirstOrDefault();

        if (firstError is null)
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An unexpected error occurred.",
                detail: "Unknown error");


        return firstError switch
        {
            PersistenceError => Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An unexpected error occurred.",
                detail: firstError.Message),

            ProductNotFoundError => Results.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Product not found.",
                detail: firstError.Message),

            ValidationError => Results.ValidationProblem(
                fluentResult.Errors.ToValidationErrorsDictionary()),

            _ => Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An unexpected error occurred.",
                detail: firstError.Message),
        };
    }

    private static Dictionary<string, string[]> ToValidationErrorsDictionary(this IReadOnlyList<IError> errors)
    {
        return errors.OfType<ValidationError>()
                     .GroupBy(e => e.PropertyName)
                     .ToDictionary(
                           group => group.Key,
                           group => group.Select(err => err.Message)
                                         .ToArray());
    }
}
