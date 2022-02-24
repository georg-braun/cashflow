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

public class AccountApiTests
{
    [Fact]
    public async Task CreateAndGet_OfAccount_IsCorrect()
    {
        // Arrange
        var client = new BackendWithSqlite().client;
        await AddAccountAsync(client, "cash");
      
        // Act
        var accounts = await GetAllAccountsAsync(client);

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
        await AddAccountAsync(client, "cash");
        var accounts = await GetAllAccountsAsync(client);

        // Act
        await AddIncomeAsync(client, accounts.First().Id, 35.50, DateOnlyExtensions.Today());
        var accountEntries = await GetAllAccountEntriesOfAccountAsync(client, accounts.First().Id);

        // Assert
        accountEntries.Should().HaveCount(1);
        accountEntries.First().Amount.Should().Be(35.50);
    }
    
     
    [Fact]
    public async Task CreateAndGet_OfSpendings_AreCorrect()
    {
        // Arrange
        var client = new BackendWithSqlite().client;
        await AddAccountAsync(client, "cash");
        await AddBudgetaryItemAsync(client, "groceries");
        var account = (await GetAllAccountsAsync(client)).First();
        var budgetaryItem = (await GetAllBudgetaryItemsAsync(client)).First();

        // Act
        await AddSpendingAsync(client, account.Id, budgetaryItem.Id, -35.50, DateOnlyExtensions.Today());
        var accountEntries = await GetAllAccountEntriesOfAccountAsync(client, account.Id);
        var spendings = await GetAllSpendingsAsync(client);

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
        await AddBudgetaryItemAsync(client, "groceries");
        await AddBudgetaryItemAsync(client, "car insurance");
        var budgetaryItems = await GetAllBudgetaryItemsAsync(client);
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
        await AddBudgetaryItemAsync(client, "groceries");
        var budgetaryItemResults = await GetAllBudgetaryItemsAsync(client);
        var budgetaryItem = budgetaryItemResults.First();
        
        // Act
        var date = DateOnlyExtensions.Today();
        await AddNewBudgetChange(client, budgetaryItem.Id, 80.5, date );
        await AddNewBudgetChange(client, budgetaryItem.Id, -60, date);
        var budgetChanges = await GetBudgetChanges(client, budgetaryItem.Id);
        
        // Assert
        var budgetChangeApiDtos = budgetChanges.ToList();
        budgetChangeApiDtos.Should().HaveCount(2);
        budgetChangeApiDtos.Should().Contain(_ => _.Amount.Equals(80.5));
        budgetChangeApiDtos.Should().Contain(_ => _.Amount.Equals(-60));
    }


    private async Task<IEnumerable<BudgetChangeApiDto>> GetBudgetChanges(HttpClient client, Guid budgetaryItemId)
    {
        var getBudgetChangesResult = await client.GetAsync($"{Route.GetBudgetChangesBase}/{budgetaryItemId}");
        var budgetChangesJson = await getBudgetChangesResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetChangeApiDto[]>(budgetChangesJson);
    }

    private async Task AddNewBudgetChange(HttpClient client, Guid budgetaryItemId, double amount, DateOnly date)
    {
        var budgetChangeDto = new AddBudgetChangeDto(budgetaryItemId, amount, date.ToDateTime(TimeOnly.MinValue));
        var json = JsonConvert.SerializeObject(budgetChangeDto);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Route.AddBudgetChange, data);
    }


    private async Task<IEnumerable<BudgetaryItemDto>> GetAllBudgetaryItemsAsync(HttpClient client)
    {
        var getAccountResult = await client.GetAsync(Route.GetAllBudgetaryItems);
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetaryItemDto[]>(accountsJson);
    }

    private async Task AddBudgetaryItemAsync(HttpClient client, string name)
    {
        var newBudgetaryItem = new AddNewBudgetaryItemDto(name);
        var json = JsonConvert.SerializeObject(newBudgetaryItem);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Route.AddBudgetaryItem, data);
    }

    private async Task AddIncomeAsync(HttpClient client, Guid accountId, double amount, DateOnly date)
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
    
    private async Task AddSpendingAsync(HttpClient client, Guid accountId, Guid budgetaryItemId, double amount,
        DateOnly date)
    {
        var income = new AddSpending(accountId, budgetaryItemId, amount, date.ToDateTime(TimeOnly.MinValue));
        var json = JsonConvert.SerializeObject(income);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Route.AddSpending, data);
    }
    
    private async Task<IEnumerable<AccountEntryApiDto>> GetAllAccountEntriesOfAccountAsync(HttpClient client,
        Guid accountId)
    {
        var getAccountResult = await client.GetAsync($"{Route.GetAccountEntriesOfAccountBase}/{accountId}");
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AccountEntryApiDto[]>(accountsJson);
    }

    
    private async Task<IEnumerable<SpendingDto>> GetAllSpendingsAsync(HttpClient client)
    {
        var getAccountResult = await client.GetAsync($"{Route.GetSpendings}");
        var spendingsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<SpendingDto[]>(spendingsJson);
    }

    private async Task<IEnumerable<AccountApiDto>> GetAllAccountsAsync(HttpClient client)
    {
        var getAccountResult = await client.GetAsync(Route.GetAllAccounts);
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AccountApiDto[]>(accountsJson);
    }
    
    private async Task AddAccountAsync(HttpClient client, string name)
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