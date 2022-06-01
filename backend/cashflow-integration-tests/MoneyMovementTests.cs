using System;
using System.Linq;
using System.Threading.Tasks;
using budget_backend_integration_tests.backend;
using FluentAssertions;
using Xunit;

namespace budet_backend_integration_tests;

public class MoneyMovementTests
{
    [Fact]
    public async Task MoneyMovementsAreAssociatedWithCorrectCategory()
    {
        // Arrange + Act
        var client = new ApiClient();
        var result = await client.AddCategory("car");
        var carCategoryId = result.Categories.First().Id;

        await client.AddMoneyMovement(500.20, new DateTime(2022, 2, 1), "Insurance", carCategoryId);
        await client.AddMoneyMovement(400.20, new DateTime(2022, 2, 1), "Fuel", carCategoryId);

        var allData = await client.GetAll();

        // Assert
        allData.Categories.Should().HaveCount(1);
        allData.MoneyMovements.Should().HaveCount(2);
        allData.MoneyMovements.All(_ => _.CategoryId.Equals(carCategoryId) && _.Id != Guid.Empty).Should().BeTrue();
        allData.MoneyMovements.Sum(_ => _.Amount).Should().Be(900.40);
        allData.MoneyMovements.All(_ => _.Note != string.Empty).Should().BeTrue();
    }
}