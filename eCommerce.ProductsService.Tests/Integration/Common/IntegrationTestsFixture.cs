using eCommerce.ProductsService.Tests.Integration.API;

namespace eCommerce.ProductsService.Tests.Integration.Common;

[CollectionDefinition("IntegrationTests")]
public class IntegrationTestsCollection : ICollectionFixture<IntegrationTestsFixture>
{
    
}

public class IntegrationTestsFixture : IAsyncLifetime
{
    public TestDatabase TestDatabase { get; }
    public ProductsApiFactory Factory { get; }
    public HttpClient Client { get; }

    public IntegrationTestsFixture()
    {
        TestDatabase = new TestDatabase();

        Factory = new ProductsApiFactory(TestDatabase);
        Client = Factory.CreateClient();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        Client.Dispose();
        Factory.Dispose();
        TestDatabase.Dispose();
        return Task.CompletedTask;
    }
}
