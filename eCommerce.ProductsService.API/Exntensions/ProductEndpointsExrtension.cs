using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Application.ServiceContracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.ProductsService.API.Exntensions;

/// <summary>
/// A static class for product endpoints extensions.
/// </summary>
public static class ProductEndpointsExrtension
{
    /// <summary>
    /// Maps all product-related endpoints (GET, POST, PUT, DELETE) to specified routes.
    /// </summary>
    /// <param name="app">The endpoint route builder of the application.</param>
    /// <returns>The <see cref="IEndpointRouteBuilder"/> for chaining calls.</returns>
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products");


        group.MapGet("/", async (IProductsService service, ILogger<Program> logger) =>
        {
            logger.LogInformation("Processing GET products request");

            var result = await service.GetProductsAsync();
            return result.ToOkApiResult();
        })
        .WithSummary("Retrieves all products.")
        .Produces(StatusCodes.Status200OK);


        group.MapGet("/{id:guid}", 
            async (
                [FromRoute] Guid id, 
                IProductsService service, 
                ILogger<Program> logger) =>
        {
            logger.LogInformation("Processing GET product request for {ProductId}", id);

            var result = await service.GetByIdAsync(id);
            return result.ToOkApiResult();
        })
        .WithSummary("Retrieves product with specified ID.")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);


        group.MapGet("/search/{searchString}", 
            async (
                [FromRoute] string searchString, 
                IProductsService service,
                ILogger<Program> logger) =>
        {
            logger.LogInformation("Proessing GET product by search request for {SearchString}", searchString);

            var result = await service.GetBySearchStringAsync(searchString);
            return result.ToOkApiResult();
        })
        .WithSummary("Retrieves product by search string.")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError);


        group.MapPost("/",
            async (
                [FromBody] AddProductRequest request,
                IProductsService service,
                IValidator<AddProductRequest> validator,
                HttpContext context,
                ILogger<Program> logger) =>
        {
            logger.LogInformation("Proessing POST add product request for {ProductName}", request.Name);

            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());

            var result = await service.AddProductAsync(request.AdaptToAddProductDto());

            string domain = $"{context.Request.Scheme}://{context.Request.Host}";
            return result.ToCreatedProductApiResult($"{domain}/api/products/");
        })
        .WithSummary("Adds a new product.")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);


        group.MapPut("/{id:guid}",
            async (
                [FromBody] UpdateProductRequest request,
                [FromRoute] Guid id,
                IProductsService service,
                IValidator<UpdateProductRequest> validator,
                ILogger<Program> logger) =>
        {
            logger.LogInformation("Proessing PUT update product request for {ProductName}", request.Name);

            if (id != request.Id)
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "ID does not match",
                    detail: "ID from body must match ID from route");

            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());

            var result = await service.UpdateProductAsync(request.AdaptToUpdateProductDto());
            return result.ToOkApiResult();
        })
        .WithSummary("Updates exisiting product with specified ID.")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);


        group.MapDelete("/{id:guid}", 
            async (
                [FromRoute] Guid id, 
                IProductsService service,
                ILogger<Program> logger) =>
        {
            logger.LogInformation("Proessing DELETE product request for {ProductId}", id);

            var result = await service.DeleteProductAsync(id);
            return result.ToNoContentApiResult();
        })
        .WithSummary("Deletes product with specified ID.")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }
}
