using System;
using System.Linq;
using System.Threading.Tasks;
using budet_backend_integration_tests.backend;
using budget_backend;
using FluentAssertions;
using Xunit;

namespace budet_backend_integration_tests;

public class AccountApiTests
{

    [Fact]
    public async Task Commands_ResultInCorrectEffects()
    {
        // Arrange + Act
        var client = new BackendWithSqlite().client;
        var changedData = await Api.AddAccountAsync(client, "cash");
        var account = changedData.Accounts.First();
        var addIncomeResult = await Api.AddIncomeAsync(client, account.Id, 50.50, DateOnlyExtensions.Today());
        
        var addBudgetaryItemResult = await Api.AddBudgetaryItemAsync(client, "groceries");
        var budgetaryItem = addBudgetaryItemResult.BudgetaryItem.First();
        
        var budgetEntryResult = await Api.AddBudgetEntry(client, budgetaryItem.Id, new DateTime(2022, 2, 1), 500.50);
        var addSpendingResult = await Api.AddSpendingAsync(client, account.Id, budgetaryItem.Id, -50, new DateTime(2022,2,1));
        
        // Assert
        addIncomeResult.AccountEntries.Should().Contain(_ => _.Amount.Equals(50.50));
        addBudgetaryItemResult.BudgetaryItem.Should().Contain(_ => _.Name.Equals("groceries"));
        budgetEntryResult.BudgetEntries.Should().Contain(_ => _.Month.Month.Equals(2) && _.Amount.Equals(500.50));
        addSpendingResult.Spendings.Should().Contain(_ =>
            _.BudgetaryItemId.Equals(budgetaryItem.Id) &&
            _.AccountEntryId.Equals(addSpendingResult.AccountEntries.First().Id));
    }
    
    [Fact]
    public async Task CreateAndGet_OfAccount_IsCorrect()
    {
        // Arrange
        var client = new BackendWithSqlite().client;
        await Api.AddAccountAsync(client, "cash");
      
        // Act
        var accounts = await Api.GetAllAccountsAsync(client);

        // Assert
        var accountFromServer = accounts.First();
        accountFromServer.Name.Should().Be("cash");
        accountFromServer.Id.Should().NotBe(Guid.Empty);
    }
    
    [Fact]
    public async Task CreateAndGet_OfTransaction_IsCorrect()
    {
        // Arrange
        var client = new BackendWithSqlite().client;
        await Api.AddAccountAsync(client, "cash");
        var accounts = await Api.GetAllAccountsAsync(client);

        // Act
        await Api.AddIncomeAsync(client, accounts.First().Id, 35.50, DateOnlyExtensions.Today());
        var accountEntries = await Api.GetAllAccountEntriesOfAccountAsync(client, accounts.First().Id);

        // Assert
        accountEntries.Should().HaveCount(1);
        accountEntries.First().Amount.Should().Be(35.50);
    }
    
     
    [Fact]
    public async Task CreateAndGet_OfSpendings_AreCorrect()
    {
        // Arrange
        var client = new BackendWithSqlite().client;
        await Api.AddAccountAsync(client, "cash");
        await Api.AddBudgetaryItemAsync(client, "groceries");
        var account = (await Api.GetAllAccountsAsync(client)).First();
        var budgetaryItem = (await Api.GetAllBudgetaryItemsAsync(client)).First();

        // Act
        await Api.AddSpendingAsync(client, account.Id, budgetaryItem.Id, -35.50, new DateTime(2022,2,1));
        var accountEntries = await Api.GetAllAccountEntriesOfAccountAsync(client, account.Id);
        var spendings = await Api.GetAllSpendingsAsync(client);

        // Assert
        accountEntries.Should().HaveCount(1);
        spendings.Should().HaveCount(1);
        accountEntries.Should().Contain(_ => _.Amount.Equals(-35.50));
    }

    [Fact]
    public async Task CreateAndGet_OfBudgetaryItems_AreCorrect()
    {
        // Arrange + Act
        var client = new BackendWithSqlite().client;
        await Api.AddBudgetaryItemAsync(client, "groceries");
        await Api.AddBudgetaryItemAsync(client, "car insurance");
        var budgetaryItems = await Api.GetAllBudgetaryItemsAsync(client);
        var budgetaryItemDtos = budgetaryItems.ToList();
        
        // Assert
        budgetaryItemDtos.Should().HaveCount(2);
        budgetaryItemDtos.Should().Contain(_ => _.Name.Equals("groceries"));
        budgetaryItemDtos.Should().Contain(_ => _.Name.Equals("car insurance"));
    }
    
    [Fact]
    public async Task CreateAndGet_OfBudgetChanges_AreCorrect()
    {
        // Arrange
        var client = new BackendWithSqlite().client;
        await Api.AddBudgetaryItemAsync(client, "groceries");
        var budgetaryItemResults = await Api.GetAllBudgetaryItemsAsync(client);
        var budgetaryItem = budgetaryItemResults.First();
        
        // Act
        var date = DateOnlyExtensions.Today();
        await Api.AddNewBudgetChange(client, budgetaryItem.Id, 80.5, date );
        await Api.AddNewBudgetChange(client, budgetaryItem.Id, -60, date);
        var budgetChanges = await Api.GetBudgetChanges(client, budgetaryItem.Id);
        
        // Assert
        var budgetChangeApiDtos = budgetChanges.ToList();
        budgetChangeApiDtos.Should().HaveCount(2);
        budgetChangeApiDtos.Should().Contain(_ => _.Amount.Equals(80.5));
        budgetChangeApiDtos.Should().Contain(_ => _.Amount.Equals(-60));
    }
}