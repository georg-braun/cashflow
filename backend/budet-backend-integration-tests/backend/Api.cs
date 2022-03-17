using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using budget_backend.Controllers;
using budget_backend.Controllers.apiDto;
using budget_backend.Controllers.apiDto.commands;
using budget_backend.Controllers.apiDto.datacontainer;
using Newtonsoft.Json;

namespace budet_backend_integration_tests.backend;

public static class Api
{
    public static async Task<IEnumerable<BudgetChangeApiDto>> GetBudgetChanges(HttpClient client, Guid budgetaryItemId)
    {
        var getBudgetChangesResult = await client.GetAsync($"{Route.GetBudgetChangesBase}/{budgetaryItemId}");
        var budgetChangesJson = await getBudgetChangesResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetChangeApiDto[]>(budgetChangesJson);
    }

    public static async Task AddNewBudgetChange(HttpClient client, Guid budgetaryItemId, double amount, DateOnly date)
    {
        var budgetChangeDto = new AddBudgetChangeDto(budgetaryItemId, amount, date.ToDateTime(TimeOnly.MinValue));
        var json = JsonConvert.SerializeObject(budgetChangeDto);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Route.SetBudgetEntry, data);
    }


    public static async Task<IEnumerable<BudgetaryItemDto>> GetAllBudgetaryItemsAsync(HttpClient client)
    {
        var getAccountResult = await client.GetAsync(Route.GetAllBudgetaryItems);
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetaryItemDto[]>(accountsJson);
    }

    public static async Task<BudgetDataApiDto> AddBudgetaryItemAsync(HttpClient client, string name)
    {
        var newBudgetaryItem = new AddNewBudgetaryItemDto(name);
        var json = JsonConvert.SerializeObject(newBudgetaryItem);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response= await client.PostAsync(Route.AddBudgetaryItem, data);
        
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetDataApiDto>(responseJson);
    }

    public static async Task<BudgetDataApiDto> AddIncomeAsync(HttpClient client, Guid accountId, double amount, DateOnly date)
    {
        var income = new AddIncomeDto()
        {
            AccountId = accountId,
            Amount = amount,
            Date = date.ToDateTime(TimeOnly.MinValue)
        };
        var json = JsonConvert.SerializeObject(income);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync(Route.AddIncome, data);
        
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetDataApiDto>(responseJson);
    }

    public static async Task<BudgetDataApiDto> AddSpendingAsync(HttpClient client, Guid accountId, Guid budgetaryItemId, double amount,
        DateTime timestamp)
    {
        var income = new AddSpending(accountId, budgetaryItemId, amount, timestamp);
        var json = JsonConvert.SerializeObject(income);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(Route.AddSpending, data);
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetDataApiDto>(responseJson);
    }

    public static async Task<IEnumerable<AccountEntryApiDto>> GetAllAccountEntriesOfAccountAsync(HttpClient client,
        Guid accountId)
    {
        var getAccountResult = await client.GetAsync($"{Route.GetAccountEntriesOfAccountBase}/{accountId}");
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AccountEntryApiDto[]>(accountsJson);
    }


    public static async Task<IEnumerable<SpendingDto>> GetAllSpendingsAsync(HttpClient client)
    {
        var getAccountResult = await client.GetAsync($"{Route.GetSpendings}");
        var spendingsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<SpendingDto[]>(spendingsJson);
    }

    public static async Task<IEnumerable<AccountApiDto>> GetAllAccountsAsync(HttpClient client)
    {
        var getAccountResult = await client.GetAsync(Route.GetAllAccounts);
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AccountApiDto[]>(accountsJson);
    }

    public static async Task<BudgetDataApiDto> AddAccountAsync(HttpClient client, string name)
    {
        var newAccount = new AddNewAccountDto()
        {
            Name = name
        };
        var json = JsonConvert.SerializeObject(newAccount);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(Route.AddAccount, data);
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetDataApiDto>(responseJson);
    }

    public static async Task<BudgetDataApiDto> GetAll(HttpClient client)
    {
        var getAllResponse = await client.GetAsync(Route.GetAll);
        var allDataAsJson = await getAllResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetDataApiDto>(allDataAsJson);
    }

    public static async Task<BudgetDataApiDto> AddBudgetEntry(HttpClient client, Guid budgetaryItemId, DateTime month, double amount)
    {
        var newAccount = new SetBudgetEntryDto(budgetaryItemId, month, amount);
   
        var json = JsonConvert.SerializeObject(newAccount);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(Route.SetBudgetEntry, data);
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetDataApiDto>(responseJson);
    }
}