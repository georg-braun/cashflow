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

        await clientA.AddAccountAsync("cashOfA");
        await clientB.AddAccountAsync("cashOfB");

        // Assert
        var accountsOfClientA = (await clientA.GetAll()).Accounts;
        accountsOfClientA.Count.Should().Be(1);
        accountsOfClientA.First().Name.Should().Be("cashOfA");
        
        var accountsOfClientB = (await clientB.GetAll()).Accounts;
        accountsOfClientB.Count.Should().Be(1);
        accountsOfClientB.First().Name.Should().Be("cashOfB");

    }
    
}