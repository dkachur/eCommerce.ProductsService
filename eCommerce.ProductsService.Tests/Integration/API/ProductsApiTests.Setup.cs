using Bogus;
using eCommerce.ProductsService.API.DTOs;
using eCommerce.ProductsService.Tests.Helpers;
using eCommerce.ProductsService.Tests.Integration.Common;
using System.Net.Http.Json;

namespace eCommerce.ProductsService.Tests.Integration.API;

[Collection("IntegrationTests")]
public partial class ProductsApiTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly TestDatabase _db;
    private readonly Faker _faker = new();

    public ProductsApiTests(IntegrationTestsFixture fixture)
    {
        _client = fixture.Client;
        _db = fixture.TestDatabase;
    }

    private async Task<List<ProductResponse>> SeedProductsAsync(int count, List<AddProductRequest>? additionalProducts = null)
    {
        var productAddRequests = Enumerable.Range(0, count).Select(_ => AddProductRequestFactory.CreateRandom()).ToList();
        if (additionalProducts is not null)
            productAddRequests.AddRange(additionalProducts);

        var productResponses = new List<ProductResponse>();

        foreach (var product in productAddRequests)
        {
            var res = await _client.PostAsJsonAsync("/api/products", product, JsonConfig.Options);
            res.EnsureSuccessStatusCode();
            var body = await res.Content.ReadAsStringAsync();
            productResponses.Add((await res.Content.ReadFromJsonAsync<ProductResponse>(JsonConfig.Options))!);
        }

        return productResponses;
    }

    public async Task InitializeAsync() => await _db.ClearDb();
    public Task DisposeAsync() => Task.CompletedTask;
}
