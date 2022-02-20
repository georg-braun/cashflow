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
        await AddNewAccountAsync("cash");
      
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
        await AddNewAccountAsync("cash");
        var accounts = await GetAllAccountsAsync();

        // Act
        await AddIncomeAsync(accounts.First().Id, 35.50, DateOnlyExtensions.Today());
        var accountEntries = await GetAllAccountEntriesOfAccount(accounts.First().Id);

        // Assert
        accountEntries.Should().HaveCount(1);
        accountEntries.First().Amount.Should().Be(35.50);
    }

    private async Task AddIncomeAsync(Guid accountId, double amount, DateOnly date)
    {
        var income = new AddIncomeDto()
        {
            AccountId = accountId,
            Amount = amount,
            Timestamp = date.ToDateTime(TimeOnly.MinValue)
        };
        var json = JsonConvert.SerializeObject(income);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Route.AddIncome, data);
    }
    
    private async Task<IEnumerable<AccountEntryApiDto>> GetAllAccountEntriesOfAccount(Guid accountId)
    {
        var getAccountResult = await client.GetAsync($"{Route.GetAccountEntriesOfAccountBase}/{accountId}");
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AccountEntryApiDto[]>(accountsJson);
    }


    private async Task<IEnumerable<AccountApiDto>> GetAllAccountsAsync()
    {
        var getAccountResult = await client.GetAsync(Route.GetAllAccounts);
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AccountApiDto[]>(accountsJson);
    }
    
    private async Task AddNewAccountAsync(string name)
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