using System;
using System.Linq;
using System.Threading.Tasks;
using budget_backend_integration_tests.backend;
using FluentAssertions;
using Xunit;

namespace budget_backend_integration_tests;

public class Scenarios
{
    /// <summary>
    ///     This test demonstrates a normal month with income, budgeted money and spendings.
    /// </summary>
    [Fact]
    public async Task RegularMonthSceanario()
    {
        // Arrange
        var client = new ApiClient();
        // add account
        var dkbAccountId = (await client.AddAccountAsync("DKB")).Accounts.First().Id;
        var cashAccountId = (await client.AddAccountAsync("Cash")).Accounts.First().Id;
        
        // add budgetary item
        var bicycleBudgetId = (await client.AddBudgetaryItemAsync("Bicycle")).BudgetaryItem.First().Id;
        var groceriesBudgetId = (await client.AddBudgetaryItemAsync("Groceries")).BudgetaryItem.First().Id;
        
        // bicycle budget
        await client.AddBudgetEntry(bicycleBudgetId, new DateTime(2022,3,1), 50);
        await client.AddBudgetEntry(bicycleBudgetId, new DateTime(2022,3,2), 50);
        
        // groceries budget
        await client.AddBudgetEntry(groceriesBudgetId, new DateTime(2022,3,2), 300);
        await client.AddBudgetEntry(groceriesBudgetId, new DateTime(2022,3,2), -50);
        
        // add cash stuff
        await client.AddIncomeAsync(cashAccountId, 200, new DateTime(2022,3,2));
        await client.AddIncomeAsync(cashAccountId, 100, new DateTime(2022,3,2));
        await client.AddSpendingAsync(cashAccountId, groceriesBudgetId, 50, new DateTime(2022,3,10));
        await client.AddIncomeAsync(cashAccountId, 50, new DateTime(2022,3,20));
        
        // add credit card stuff
        await client.AddIncomeAsync(dkbAccountId, 1000, new DateTime(2022,3,2));
        await client.AddSpendingAsync(dkbAccountId, bicycleBudgetId, 150, new DateTime(2022,3,11));
        await client.AddSpendingAsync(dkbAccountId, bicycleBudgetId, 250, new DateTime(2022,3,12));

        // Act
        var allData = await client.GetAll();
        
        // Assert
        allData.AccountEntries.Where(_ => _.AccountId.Equals(cashAccountId)).Sum(_ => _.Amount).Should().Be(300);
        allData.AccountEntries.Where(_ => _.AccountId.Equals(dkbAccountId)).Sum(_ => _.Amount).Should().Be(600);
        
        // groceries budget
        var groceriesBudget = allData.BudgetEntries.Where(_ => _.BudgetaryItemId.Equals(groceriesBudgetId)).Sum(_ => _.Amount);
        groceriesBudget.Should().Be(250);
        
        var groceriesSpendings = allData.Spendings
            .Where(_ => _.BudgetaryItemId.Equals(groceriesBudgetId))
            .Sum(_ => allData.AccountEntries.First(accountEntry => accountEntry.Id.Equals(_.AccountEntryId)).Amount);
        // spendings have negative numbers => addition
        var availableGroceriesBudget = groceriesBudget + groceriesSpendings;
        availableGroceriesBudget.Should().Be(200);
        
        // bicycle budget
        var bicycleBudget = allData.BudgetEntries.Where(_ => _.BudgetaryItemId.Equals(bicycleBudgetId))
            .Sum(_ => _.Amount);
        bicycleBudget.Should().Be(100);

        var bicycleSpendings = allData.Spendings
            .Where(_ => _.BudgetaryItemId.Equals(bicycleBudgetId))
            .Sum(_ => allData.AccountEntries.First(accountEntry => accountEntry.Id.Equals(_.AccountEntryId)).Amount);
        // spendings have negative numbers => addition
        var availableBicycleBudget = bicycleBudget + bicycleSpendings;
        availableBicycleBudget.Should().Be(-300);
        
        // available money
        var totalBalance = allData.AccountEntries.Sum(_ => _.Amount);
        totalBalance.Should().Be(900);

        // remaining money to budget
        var totalBudgeted = allData.BudgetEntries.Sum(_ => _.Amount);
        var totalSpent  = allData.Spendings
            .Sum(_ => allData.AccountEntries.First(accountEntry => accountEntry.Id.Equals(_.AccountEntryId)).Amount);
        var currentBudgeted = totalBudgeted + totalSpent;
        var availableBudget = totalBalance - currentBudgeted;
        availableBudget.Should().Be(1000);
    }
    
}