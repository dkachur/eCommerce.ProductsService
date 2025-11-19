namespace eCommerce.ProductsService.Tests.Integration.Common;

[CollectionDefinition("IntegrationTests")]
public class IntegrationTestsCollection : ICollectionFixture<IntegrationTestsFixture>
{
    
}

public class IntegrationTestsFixture : IAsyncLifetime
{
    public TestDatabase TestDatabase { get; }

    public IntegrationTestsFixture()
    {
        TestDatabase = new TestDatabase();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        TestDatabase.Dispose();
        return Task.CompletedTask;
    }
}