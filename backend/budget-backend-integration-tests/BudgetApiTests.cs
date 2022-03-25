using System;
using System.Linq;
using System.Threading.Tasks;
using budet_backend_integration_tests.backend;
using budget_backend;
using FluentAssertions;
using Xunit;

namespace budet_backend_integration_tests;

public class BudgetApiTests
{
    [Fact]
    public async Task SavingMoneyAndSpendingForCarInsurance_IsCorrect()
    {
        // Arrange
        var client = new ApiClient();
        await client.AddAccountAsync("cash");
        var account = (await client.GetAllAccountsAsync()).First();
        await client.AddIncomeAsync(account.Id, 49.50, DateOnlyExtensions.Today());
        await client.AddIncomeAsync(account.Id, 50.50, DateOnlyExtensions.Today());
        
        await client.AddBudgetaryItemAsync("car insurance");
        var budgetaryItem = (await client.GetAllBudgetaryItemsAsync()).First();
        await client.AddNewBudgetChange(budgetaryItem.Id, 200, DateOnlyExtensions.Today());
        await client.AddNewBudgetChange(budgetaryItem.Id, 100, DateOnlyExtensions.Today());
        await client.AddNewBudgetChange(budgetaryItem.Id, -50, DateOnlyExtensions.Today());

        // Act
        

        // Assert

    }
    
}