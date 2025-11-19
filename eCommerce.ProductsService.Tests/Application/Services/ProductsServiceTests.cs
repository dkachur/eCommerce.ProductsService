using AutoFixture;
using Bogus;
using eCommerce.ProductsService.Application.DTOs;
using eCommerce.ProductsService.Application.Enums;
using eCommerce.ProductsService.Application.Messaging;
using eCommerce.ProductsService.Application.RepositoryContracts;
using eCommerce.ProductsService.Domain.Entities;
using eCommerce.ProductsService.Tests.Helpers;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsAppService = eCommerce.ProductsService.Application.Services.ProductsService;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace eCommerce.ProductsService.Tests.Application.Services;

public partial class ProductsServiceTests
{
    private readonly ProductsAppService _service;

    private readonly Mock<IProductsRepository> _repoMock;
    private readonly Mock<IValidator<AddProductDto>> _addValidatorMock;
    private readonly Mock<IValidator<UpdateProductDto>> _updateValidatorMock;
    private readonly Mock<ILogger<ProductsAppService>> _loggerMock;
    private readonly Mock<IMessagePublisher<ProductUpdatedMessage>> _updatePublisherMock;
    private readonly Mock<IMessagePublisher<ProductDeletedMessage>> _deletePublisherMock;

    private readonly Fixture _fixture;
    private readonly Faker _faker;

    private const int ValidProductListCount = 10;
    private const int ProductFromSearchCount = 4;
    private const int ProductsByIdsCount = 6;

    private readonly List<Product> _validProducts;
    private readonly List<Product> _productsFromSearch;
    private readonly List<Product> _productsByIds;

    public ProductsServiceTests()
    {
        _fixture = new Fixture();
        _faker = new Faker();

        _repoMock = new Mock<IProductsRepository>();
        _addValidatorMock = new Mock<IValidator<AddProductDto>>();
        _updateValidatorMock = new Mock<IValidator<UpdateProductDto>>();
        _loggerMock = new Mock<ILogger<ProductsAppService>>();
        _updatePublisherMock = new Mock<IMessagePublisher<ProductUpdatedMessage>>();
        _deletePublisherMock = new Mock<IMessagePublisher<ProductDeletedMessage>>();

        _addValidatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AddProductDto>(), default))
            .ReturnsAsync(new ValidationResult());

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<UpdateProductDto>(), default))
            .ReturnsAsync(new ValidationResult());


        _repoMock
            .Setup(r => r.AddProductAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product p) => p);

        _repoMock
            .Setup(r => r.UpdateProductAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product p) => p);

        _repoMock
            .Setup(r => r.GetProductByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(ProductFactory.CreateRandom());

        _validProducts = Enumerable
            .Range(0, ValidProductListCount)
            .Select(_ => ProductFactory.CreateRandom())
            .ToList();

        _repoMock
            .Setup(r => r.GetProductsAsync())
            .ReturnsAsync(_validProducts);

        _productsFromSearch = Enumerable
            .Range(0, ProductFromSearchCount)
            .Select(_ => ProductFactory.CreateRandom())
            .ToList();

        _repoMock
            .Setup(r => r.GetBySearchStringAsync(It.IsAny<string>()))
            .ReturnsAsync(_productsFromSearch);

        _productsByIds = Enumerable
            .Range(0, ProductsByIdsCount)
            .Select(_ => ProductFactory.CreateRandom())
            .ToList();

        _repoMock
            .Setup(r => r.GetProductsByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(_productsByIds);

        _repoMock
            .Setup(r => r.DeleteProductAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _repoMock
            .Setup(r => r.GetExistingProductIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync((IEnumerable<Guid> ids) => ids);


        _service = new ProductsAppService(
            _repoMock.Object,
            _addValidatorMock.Object,
            _updateValidatorMock.Object,
            _loggerMock.Object,
            _updatePublisherMock.Object,
            _deletePublisherMock.Object);
    }

    

    #region Helpers

    private static void SetupValidatorMock<TDto>(Mock<IValidator<TDto>> validator, string propertyName, string message)
    {
        var validationFailure = new ValidationFailure(propertyName, message);

        validator
            .Setup(v => v.ValidateAsync(It.IsAny<TDto>(), default))
            .ReturnsAsync(new ValidationResult([validationFailure]));
    }

    #endregion
}
