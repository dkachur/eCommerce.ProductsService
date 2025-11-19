using FluentAssertions;
using Moq;

namespace eCommerce.ProductsService.Tests.Application.Services;

public partial class ProductsServiceTests
{
    [Fact(DisplayName = "CheckProductsExistAsync should return all true when all products exist")]
    public async Task CheckProductsExistAsync_ShouldReturnAllTrue_WhenAllExist()
    {
        // Arrange
        var idsCount = _faker.Random.Int(3, 50);
        var ids = CreateRandomGuidList(idsCount);


        // Act
        var result = await _service.CheckProductsExistAsync(ids);


        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        foreach (var id in ids)
        {
            result.Value.Should().Contain(id, true);
        }
    }

    [Fact(DisplayName = "CheckProductsExistAsync should return correct true/false mapping when some products exist")]
    public async Task CheckProductsExistAsync_ShouldReturnMixed_WhenSomeExist()
    {
        // Arrange
        var idsCount = _faker.Random.Int(3, 50);
        var ids = CreateRandomGuidList(idsCount);
        var existingIds = _faker.Random.ListItems(ids, _faker.Random.Int(1, idsCount - 1));
        _repoMock
            .Setup(r => r.GetExistingProductIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(existingIds);


        // Act 
        var result = await _service.CheckProductsExistAsync(ids);


        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        foreach (var id in ids)
        {
            result.Value.Should().Contain(id, existingIds.Contains(id));
        }
    }

    [Fact(DisplayName = "CheckProductsExistAsync should return all false when none exist")]
    public async Task CheckProductsExistAsync_ShouldReturnAllFalse_WhenNoneExist()
    {
        // Arrange
        var idsCount = _faker.Random.Int(3, 50);
        var ids = CreateRandomGuidList(idsCount);

        _repoMock
            .Setup(r => r.GetExistingProductIdsAsync(ids))
            .ReturnsAsync([]);


        // Act
        var result = await _service.CheckProductsExistAsync(ids);


        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        foreach (var id in ids)
        {
            result.Value.Should().Contain(id, false);
        }
    }

    [Fact(DisplayName = "CheckProductsExistAsync should return empty dictionary when input is empty")]
    public async Task CheckProductsExistAsync_ShouldReturnEmpty_WhenInputIsEmpty()
    {
        // Arrange
        var idsCount = 0;
        var ids = CreateRandomGuidList(idsCount);


        // Act
        var result = await _service.CheckProductsExistAsync(ids);


        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact(DisplayName = "CheckProductsExistAsync should remove duplicates from input")]
    public async Task CheckProductsExistAsync_ShouldRemoveDuplicates_WhenInputHasDuplicateIds()
    {
        // Arrange
        var idsCount = 20;
        var ids = CreateRandomGuidList(idsCount);

        var idsWithDuplicates = new List<Guid>(ids);

        var extraDuplicatesCount = 10;
        for (int i = 0; i < extraDuplicatesCount; i++)
        {
            idsWithDuplicates.Add(_faker.Random.ListItem(ids));
        }


        // Act
        var result = await _service.CheckProductsExistAsync(idsWithDuplicates);


        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        result.Value.Count.Should().Be(idsCount);
        foreach(var id in ids)
        {
            result.Value.Should().Contain(id, true);
        }
    }


    #region Helpers

    private List<Guid> CreateRandomGuidList(int count)
        => Enumerable.Range(0, count)
            .Select(i => Guid.NewGuid())
            .ToList();

    #endregion
}
