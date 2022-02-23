using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using budget_backend;
using budget_backend.Controllers;
using budget_backend.Controllers.apiDto;
using budget_backend.Controllers.apiDto.commands;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace budet_backend_integration_tests;

public class AccountApiTests : IntegrationTest
{
    [Fact]
    public async Task CreateAndGet_OfAccount_IsCorrect()
    {
        // Arrange
        await AddAccountAsync("cash");
      
        // Act
        var accounts = await GetAllAccountsAsync();

        // Assert
        var accountFromServer = accounts.First();
        accountFromServer.Name.Should().Be("cash");
        accountFromServer.Id.Should().NotBe(Guid.Empty);
    }
    
    [Fact]
    public async Task CreateAndGet_OfTransaction_IsCorrect()
    {
        // Arrange
        await AddAccountAsync("cash");
        var accounts = await GetAllAccountsAsync();

        // Act
        await AddIncomeAsync(accounts.First().Id, 35.50, DateOnlyExtensions.Today());
        var accountEntries = await GetAllAccountEntriesOfAccountAsync(accounts.First().Id);

        // Assert
        accountEntries.Should().HaveCount(1);
        accountEntries.First().Amount.Should().Be(35.50);
    }
    
     
    [Fact]
    public async Task CreateAndGet_OfSpendings_AreCorrect()
    {
        // Arrange
        await AddAccountAsync("cash");
        await AddBudgetaryItemAsync("groceries");
        var account = (await GetAllAccountsAsync()).First();
        var budgetaryItem = (await GetAllBudgetaryItemsAsync()).First();

        // Act
        await AddSpendingAsync(account.Id, budgetaryItem.Id, -35.50, DateOnlyExtensions.Today());
        var accountEntries = await GetAllAccountEntriesOfAccountAsync(account.Id);
        var spendings = await GetAllSpendingsAsync();

        // Assert
        accountEntries.Should().HaveCount(1);
        spendings.Should().HaveCount(1);
        accountEntries.Should().Contain(_ => _.Amount.Equals(-35.50));
    }

    [Fact]
    public async Task CreateAndGet_OfBudgetaryItems_AreCorrect()
    {
        // Arrange + Act
        await AddBudgetaryItemAsync("groceries");
        await AddBudgetaryItemAsync("car insurance");
        var budgetaryItems = await GetAllBudgetaryItemsAsync();
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
        await AddBudgetaryItemAsync("groceries");
        var budgetaryItemResults = await GetAllBudgetaryItemsAsync();
        var budgetaryItem = budgetaryItemResults.First();
        
        // Act
        var date = DateOnlyExtensions.Today();
        await AddNewBudgetChange(budgetaryItem.Id, 80.5, date );
        await AddNewBudgetChange(budgetaryItem.Id, -60, date);
        var budgetChanges = await GetBudgetChanges(budgetaryItem.Id);
        
        // Assert
        var budgetChangeApiDtos = budgetChanges.ToList();
        budgetChangeApiDtos.Should().HaveCount(2);
        budgetChangeApiDtos.Should().Contain(_ => _.Amount.Equals(80.5));
        budgetChangeApiDtos.Should().Contain(_ => _.Amount.Equals(-60));
    }


    private async Task<IEnumerable<BudgetChangeApiDto>> GetBudgetChanges(Guid budgetaryItemId)
    {
        var getBudgetChangesResult = await client.GetAsync($"{Route.GetBudgetChangesBase}/{budgetaryItemId}");
        var budgetChangesJson = await getBudgetChangesResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetChangeApiDto[]>(budgetChangesJson);
    }

    private async Task AddNewBudgetChange(Guid budgetaryItemId, double amount, DateOnly date)
    {
        var budgetChangeDto = new AddBudgetChangeDto(budgetaryItemId, amount, date.ToDateTime(TimeOnly.MinValue));
        var json = JsonConvert.SerializeObject(budgetChangeDto);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Route.AddBudgetChange, data);
    }


    private async Task<IEnumerable<BudgetaryItemDto>> GetAllBudgetaryItemsAsync()
    {
        var getAccountResult = await client.GetAsync(Route.GetAllBudgetaryItems);
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetaryItemDto[]>(accountsJson);
    }

    private async Task AddBudgetaryItemAsync(string name)
    {
        var newBudgetaryItem = new AddNewBudgetaryItemDto(name);
        var json = JsonConvert.SerializeObject(newBudgetaryItem);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Route.AddBudgetaryItem, data);
    }

    private async Task AddIncomeAsync(Guid accountId, double amount, DateOnly date)
    {
        var income = new AddIncomeDto()
        {
            AccountId = accountId,
            Amount = amount,
            Date = date.ToDateTime(TimeOnly.MinValue)
        };
        var json = JsonConvert.SerializeObject(income);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Route.AddIncome, data);
    }
    
    private async Task AddSpendingAsync(Guid accountId, Guid budgetaryItemId, double amount, DateOnly date)
    {
        var income = new AddSpending(accountId, budgetaryItemId, amount, date.ToDateTime(TimeOnly.MinValue));
        var json = JsonConvert.SerializeObject(income);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Route.AddSpending, data);
    }
    
    private async Task<IEnumerable<AccountEntryApiDto>> GetAllAccountEntriesOfAccountAsync(Guid accountId)
    {
        var getAccountResult = await client.GetAsync($"{Route.GetAccountEntriesOfAccountBase}/{accountId}");
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AccountEntryApiDto[]>(accountsJson);
    }

    
    private async Task<IEnumerable<SpendingDto>> GetAllSpendingsAsync()
    {
        var getAccountResult = await client.GetAsync($"{Route.GetSpendings}");
        var spendingsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<SpendingDto[]>(spendingsJson);
    }

    private async Task<IEnumerable<AccountApiDto>> GetAllAccountsAsync()
    {
        var getAccountResult = await client.GetAsync(Route.GetAllAccounts);
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AccountApiDto[]>(accountsJson);
    }
    
    private async Task AddAccountAsync(string name)
    {
        var newAccount = new AddNewAccountDto()
        {
            Name = name
        };
        var json = JsonConvert.SerializeObject(newAccount);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Route.AddAccount, data);
    }
}