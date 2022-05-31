using System;
using System.Linq;
using System.Threading.Tasks;
using budget_backend_integration_tests.backend;
using FluentAssertions;
using Xunit;

namespace budget_backend_integration_tests;

public class MultipleUserScenarioTests
{
    [Fact]
    public async Task AccountsOfMultipleClientsAreSeparated()
    {
        // Arrange
        // a new client has it's own token.
        var clientA = new ApiClient();
        var clientB = new ApiClient();
        // Act

        await clientA.AddCategory("carA");
        await clientB.AddCategory("carB");

        // Assert
        var categoriesOfClientA = (await clientA.GetAll()).Categories;
        categoriesOfClientA.Count.Should().Be(1);
        categoriesOfClientA.First().Name.Should().Be("carA");

        var categoriesOfClientB = (await clientB.GetAll()).Categories;
        categoriesOfClientB.Count.Should().Be(1);
        categoriesOfClientB.First().Name.Should().Be("carB");
    }
}