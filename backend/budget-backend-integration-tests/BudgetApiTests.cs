using System;
using System.Linq;
using System.Threading.Tasks;
using budget_backend;
using budget_backend_integration_tests.backend;
using FluentAssertions;
using Xunit;

namespace budget_backend_integration_tests;

public class BudgetApiTests
{
    [Fact]
    public async Task SavingMoneyAndSpendingForCarInsurance_IsCorrect()
    {
        // Arrange
        var client = new ApiClient();
        await client.AddAccountAsync("cash");
        var account = (await client.GetAllAccountsAsync()).First();
        await client.AddAccountEntryAsync(account.Id, 49.50, DateTime.Today);
        await client.AddAccountEntryAsync(account.Id, 50.50, DateTime.Today);
        
        await client.AddBudgetaryItemAsync("car insurance");
        var budgetaryItem = (await client.GetAllBudgetaryItemsAsync()).First();
        await client.AddNewBudgetChange(budgetaryItem.Id, 200, DateOnlyExtensions.Today());
        await client.AddNewBudgetChange(budgetaryItem.Id, 100, DateOnlyExtensions.Today());
        await client.AddNewBudgetChange(budgetaryItem.Id, -50, DateOnlyExtensions.Today());

        // Act
        

        // Assert

    }
    
}