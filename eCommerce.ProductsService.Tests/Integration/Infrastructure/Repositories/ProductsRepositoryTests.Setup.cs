using AutoFixture;
using Bogus;
using eCommerce.ProductsService.Infrastructure.DbContext;
using eCommerce.ProductsService.Infrastructure.Repositories;
using eCommerce.ProductsService.Tests.Integration.Common;

namespace eCommerce.ProductsService.Tests.Integration.Infrastructure.Repositories;

[Collection("IntegrationTests")]
public partial class ProductsRepositoryTests: IAsyncLifetime
{
    private readonly ProductsRepository _repo;
    private readonly TestDatabase _db;
    private readonly Fixture _fixture = new();
    private readonly Faker _faker = new();

    public ProductsRepositoryTests(IntegrationTestsFixture fixture)
    {
        _db = fixture.TestDatabase;

        var dbContext = new DapperDbContext(_db.Connection);
        _repo = new(dbContext);
    }

    public async Task InitializeAsync() => await _db.ClearDb();
    public Task DisposeAsync() => Task.CompletedTask;
}
