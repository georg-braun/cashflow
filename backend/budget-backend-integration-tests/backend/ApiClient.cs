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

namespace budget_backend_integration_tests.backend;

/// <summary>
///     Client for interaction with the backend api.
/// </summary>
public class ApiClient
{
    private HttpClient client; 
    public ApiClient()
    {
        client = new BackendWithSqlite().client;
    }
    
    
    public async Task<IEnumerable<BudgetChangeApiDto>> GetBudgetChanges(Guid budgetaryItemId)
    {
        var getBudgetChangesResult = await client.GetAsync($"{Routes.GetBudgetChangesBase}/{budgetaryItemId}");
        var budgetChangesJson = await getBudgetChangesResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetChangeApiDto[]>(budgetChangesJson);
    }

    public async Task AddNewBudgetChange(Guid budgetaryItemId, double amount, DateOnly date)
    {
        var budgetChangeDto = new AddBudgetChangeDto(budgetaryItemId, amount, date.ToDateTime(TimeOnly.MinValue));
        var json = JsonConvert.SerializeObject(budgetChangeDto);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Routes.SetBudgetEntry, data);
    }


    public async Task<IEnumerable<BudgetaryItemDto>> GetAllBudgetaryItemsAsync()
    {
        var getAccountResult = await client.GetAsync(Routes.GetAllBudgetaryItems);
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetaryItemDto[]>(accountsJson);
    }

    public async Task<BudgetDataApiDto> AddBudgetaryItemAsync(string name)
    {
        var newBudgetaryItem = new AddNewBudgetaryItemDto(name);
        var json = JsonConvert.SerializeObject(newBudgetaryItem);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response= await client.PostAsync(Routes.AddBudgetaryItem, data);
        
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetDataApiDto>(responseJson);
    }

    public async Task<BudgetDataApiDto> AddIncomeAsync(Guid accountId, double amount, DateOnly date)
    {
        var income = new AddIncomeDto()
        {
            AccountId = accountId,
            Amount = amount,
            Date = date.ToDateTime(TimeOnly.MinValue)
        };
        var json = JsonConvert.SerializeObject(income);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync(Routes.AddIncome, data);
        
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetDataApiDto>(responseJson);
    }
    
    public async Task<IEnumerable<AccountEntryApiDto>> GetAllAccountEntriesOfAccountAsync(Guid accountId)
    {
        var getAccountResult = await client.GetAsync($"{Routes.GetAccountEntriesOfAccountBase}/{accountId}");
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AccountEntryApiDto[]>(accountsJson);
    }


    public async Task<IEnumerable<SpendingDto>> GetAllSpendingsAsync()
    {
        var getAccountResult = await client.GetAsync($"{Routes.GetSpendings}");
        var spendingsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<SpendingDto[]>(spendingsJson);
    }

    public async Task<IEnumerable<AccountApiDto>> GetAllAccountsAsync()
    {
        var getAccountResult = await client.GetAsync(Routes.GetAllAccounts);
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AccountApiDto[]>(accountsJson);
    }
    

    public async Task<BudgetDataApiDto> GetAll()
    {
        var getAllResponse = await client.GetAsync(Routes.GetAll);
        var allDataAsJson = await getAllResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetDataApiDto>(allDataAsJson);
    }

    public async Task<BudgetDataApiDto> AddBudgetEntry(Guid budgetaryItemId, DateTime month, double amount)
    {
        var newAccount = new SetBudgetEntryDto(budgetaryItemId, month, amount);
   
        var json = JsonConvert.SerializeObject(newAccount);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(Routes.SetBudgetEntry, data);
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetDataApiDto>(responseJson);
    }
    
    public static async Task<IEnumerable<BudgetChangeApiDto>> GetBudgetChanges(HttpClient client, Guid budgetaryItemId)
    {
        var getBudgetChangesResult = await client.GetAsync($"{Routes.GetBudgetChangesBase}/{budgetaryItemId}");
        var budgetChangesJson = await getBudgetChangesResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetChangeApiDto[]>(budgetChangesJson);
    }

    public static async Task AddNewBudgetChange(HttpClient client, Guid budgetaryItemId, double amount, DateOnly date)
    {
        var budgetChangeDto = new AddBudgetChangeDto(budgetaryItemId, amount, date.ToDateTime(TimeOnly.MinValue));
        var json = JsonConvert.SerializeObject(budgetChangeDto);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Routes.SetBudgetEntry, data);
    }


    public static async Task<IEnumerable<BudgetaryItemDto>> GetAllBudgetaryItemsAsync(HttpClient client)
    {
        var getAccountResult = await client.GetAsync(Routes.GetAllBudgetaryItems);
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetaryItemDto[]>(accountsJson);
    }

    public static async Task<BudgetDataApiDto> AddBudgetaryItemAsync(HttpClient client, string name)
    {
        var newBudgetaryItem = new AddNewBudgetaryItemDto(name);
        var json = JsonConvert.SerializeObject(newBudgetaryItem);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response= await client.PostAsync(Routes.AddBudgetaryItem, data);
        
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
        
        var response = await client.PostAsync(Routes.AddIncome, data);
        
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetDataApiDto>(responseJson);
    }

    public async Task<BudgetDataApiDto> AddSpendingAsync(Guid accountId, Guid budgetaryItemId, double amount,
        DateTime timestamp)
    {
        var income = new AddSpending(accountId, budgetaryItemId, amount, timestamp);
        var json = JsonConvert.SerializeObject(income);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(Routes.AddSpending, data);
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetDataApiDto>(responseJson);
    }
    
    public async Task<BudgetDataApiDto> AddAccountAsync(string name)
    {
        var newAccount = new AddNewAccountDto()
        {
            Name = name
        };
        var json = JsonConvert.SerializeObject(newAccount);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(Routes.AddAccount, data);
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetDataApiDto>(responseJson);
    }

}