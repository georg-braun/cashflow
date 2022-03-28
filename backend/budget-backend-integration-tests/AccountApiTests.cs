using System;
using System.Linq;
using System.Threading.Tasks;
using budget_backend;
using budget_backend_integration_tests.backend;
using FluentAssertions;
using Xunit;

namespace budget_backend_integration_tests;

public class AccountApiTests
{
    [Fact]
    public async Task BudgetEntriesResultInCorrectBudget()
    {
        // Arrange + Act
        var client = new ApiClient();
        var addBudgetaryItemResult = await client.AddBudgetaryItemAsync( "groceries");
        var budgetaryItem = addBudgetaryItemResult.BudgetaryItem.First();
        
        await client.AddBudgetEntry( budgetaryItem.Id, new DateTime(2022, 2, 1), 500.50);
        await client.AddBudgetEntry( budgetaryItem.Id, new DateTime(2022, 3, 1), 500.50);

        var allData = await client.GetAll();
     
        // Assert
        var budgeted = allData.BudgetEntries.Sum(_ => _.Amount);
        budgeted.Should().Be(1001);
    }
    
    [Fact]
    public async Task BudgetEntriesAndSpendingResultInCorrectAvailableBudget()
    {
        // Arrange + Act
        var client = new ApiClient();
        var changedData = await client.AddAccountAsync( "cash");
        var account = changedData.Accounts.First();
        
        var addBudgetaryItemResult = await client.AddBudgetaryItemAsync( "groceries");
        var groceriesBudgetaryItem = addBudgetaryItemResult.BudgetaryItem.First();
        
        await client.AddBudgetEntry( groceriesBudgetaryItem.Id, new DateTime(2022, 2, 1), 500);
        await client.AddBudgetEntry( groceriesBudgetaryItem.Id, new DateTime(2022, 3, 1), 500);

        await client.AddSpendingAsync( account.Id, groceriesBudgetaryItem.Id, -200, new DateTime(2022, 2, 10));

        var allData = await client.GetAll();
        var budgeted = allData.BudgetEntries.Where(_ => _.BudgetaryItemId.Equals(groceriesBudgetaryItem.Id)).Sum(_ => _.Amount);
        var groceriesSpendings = allData.Spendings.Where(_ => _.BudgetaryItemId.Equals(groceriesBudgetaryItem.Id));
        var groceriesSpendingAccountEntries = groceriesSpendings.Select(_ =>
            allData.AccountEntries.FirstOrDefault(accountEntry => accountEntry.Id.Equals(_.AccountEntryId)));
        var groceriesSpendingsTotal = groceriesSpendingAccountEntries.Sum(_ =>
        {
            if (_ != null) return _.Amount;
            return 0;
        });
        var availableBudgetForGroceries = budgeted + groceriesSpendingsTotal;
        
        // Assert
        availableBudgetForGroceries.Should().Be(800);
    }
    
    [Fact]
    public async Task Commands_ResultInCorrectEffects()
    {
        // Arrange + Act
        var client = new ApiClient();
        var changedData = await client.AddAccountAsync( "cash");
        var account = changedData.Accounts.First();
        var addIncomeResult = await client.AddIncomeAsync(account.Id, 50.50, DateOnlyExtensions.Today());
        
        var addBudgetaryItemResult = await client.AddBudgetaryItemAsync( "groceries");
        var budgetaryItem = addBudgetaryItemResult.BudgetaryItem.First();
        
        var budgetEntryResult = await client.AddBudgetEntry( budgetaryItem.Id, new DateTime(2022, 2, 1), 500.50);
        var addSpendingResult = await client.AddSpendingAsync(account.Id, budgetaryItem.Id, -50, new DateTime(2022,2,1));
        
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
        var client = new ApiClient();
        await client.AddAccountAsync("cash");
      
        // Act
        var accounts = await client.GetAllAccountsAsync();

        // Assert
        var accountFromServer = accounts.First();
        accountFromServer.Name.Should().Be("cash");
        accountFromServer.Id.Should().NotBe(Guid.Empty);
    }
    
    [Fact]
    public async Task CreateAndGet_OfTransaction_IsCorrect()
    {
        // Arrange
        var client = new ApiClient();
        await client.AddAccountAsync("cash");
        var accounts = await client.GetAllAccountsAsync();

        // Act
        await client.AddIncomeAsync(accounts.First().Id, 35.50, DateOnlyExtensions.Today());
        var accountEntries = await client.GetAllAccountEntriesOfAccountAsync(accounts.First().Id);

        // Assert
        accountEntries.Should().HaveCount(1);
        accountEntries.First().Amount.Should().Be(35.50);
    }
    
     
    [Fact]
    public async Task CreateAndGet_OfSpendings_AreCorrect()
    {
        // Arrange
        var client = new ApiClient();
        await client.AddAccountAsync("cash");
        await client.AddBudgetaryItemAsync("groceries");
        var account = (await client.GetAllAccountsAsync()).First();
        var budgetaryItem = (await client.GetAllBudgetaryItemsAsync()).First();

        // Act
        await client.AddSpendingAsync(account.Id, budgetaryItem.Id, -35.50, new DateTime(2022,2,1));
        var accountEntries = await client.GetAllAccountEntriesOfAccountAsync(account.Id);
        var spendings = await client.GetAllSpendingsAsync();

        // Assert
        accountEntries.Should().HaveCount(1);
        spendings.Should().HaveCount(1);
        accountEntries.Should().Contain(_ => _.Amount.Equals(-35.50));
    }

    [Fact]
    public async Task CreateAndGet_OfBudgetaryItems_AreCorrect()
    {
        // Arrange + Act
        var client = new ApiClient();
        await client.AddBudgetaryItemAsync("groceries");
        await client.AddBudgetaryItemAsync("car insurance");
        var budgetaryItems = await client.GetAllBudgetaryItemsAsync();
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
        var client = new ApiClient();
        await client.AddBudgetaryItemAsync("groceries");
        var budgetaryItemResults = await client.GetAllBudgetaryItemsAsync();
        var budgetaryItem = budgetaryItemResults.First();
        
        // Act
        var date = DateOnlyExtensions.Today();
        await client.AddNewBudgetChange(budgetaryItem.Id, 80.5, date );
        await client.AddNewBudgetChange(budgetaryItem.Id, -60, date);
        var budgetChanges = await client.GetBudgetChanges(budgetaryItem.Id);
        
        // Assert
        var budgetChangeApiDtos = budgetChanges.ToList();
        budgetChangeApiDtos.Should().HaveCount(2);
        budgetChangeApiDtos.Should().Contain(_ => _.Amount.Equals(80.5));
        budgetChangeApiDtos.Should().Contain(_ => _.Amount.Equals(-60));
    }
}