using AutoFixture;
using Bogus;
using eCommerce.ProductsService.Infrastructure.DbContext;
using eCommerce.ProductsService.Infrastructure.Repositories;
using eCommerce.ProductsService.Tests.Integration.Infrastructure.Common;

namespace eCommerce.ProductsService.Tests.Integration.Infrastructure.Repositories;

public partial class ProductsRepositoryTests
{
    private readonly ProductsRepository _repo;
    private readonly Fixture _fixture;
    private readonly Faker _faker;

    public ProductsRepositoryTests()
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        _fixture = new Fixture();
        _faker = new Faker();

        var testDb = new TestDatabase();
        var dbContext = new DapperDbContext(testDb.Connection);
        _repo = new(dbContext);
    }
}
