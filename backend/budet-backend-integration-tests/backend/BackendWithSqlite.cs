using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using budget_backend.Controllers;
using budget_backend.Controllers.apiDto;
using budget_backend.Controllers.apiDto.commands;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace budet_backend_integration_tests.backend;


public class ApiClient
{
    private HttpClient client; 
    public ApiClient()
    {
        client = new BackendWithSqlite().client;
    }

    public async Task<IEnumerable<BudgetChangeApiDto>> GetBudgetChanges(Guid budgetaryItemId)
    {
        var getBudgetChangesResult = await client.GetAsync($"{Route.GetBudgetChangesBase}/{budgetaryItemId}");
        var budgetChangesJson = await getBudgetChangesResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetChangeApiDto[]>(budgetChangesJson);
    }

    public async Task AddNewBudgetChange(Guid budgetaryItemId, double amount, DateOnly date)
    {
        var budgetChangeDto = new AddBudgetChangeDto(budgetaryItemId, amount, date.ToDateTime(TimeOnly.MinValue));
        var json = JsonConvert.SerializeObject(budgetChangeDto);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Route.SetBudgetEntry, data);
    }


    public async Task<IEnumerable<BudgetaryItemDto>> GetAllBudgetaryItemsAsync()
    {
        var getAccountResult = await client.GetAsync(Route.GetAllBudgetaryItems);
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BudgetaryItemDto[]>(accountsJson);
    }

    public async Task AddBudgetaryItemAsync(string name)
    {
        var newBudgetaryItem = new AddNewBudgetaryItemDto(name);
        var json = JsonConvert.SerializeObject(newBudgetaryItem);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Route.AddBudgetaryItem, data);
    }

    public async Task AddIncomeAsync(Guid accountId, double amount, DateOnly date)
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

    public async Task AddSpendingAsync(Guid accountId, Guid budgetaryItemId, double amount,
        DateOnly date)
    {
        var income = new AddSpending(accountId, budgetaryItemId, amount, date.ToDateTime(TimeOnly.MinValue));
        var json = JsonConvert.SerializeObject(income);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Route.AddSpending, data);
    }

    public async Task<IEnumerable<AccountEntryApiDto>> GetAllAccountEntriesOfAccountAsync(Guid accountId)
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

    public async Task<IEnumerable<AccountApiDto>> GetAllAccountsAsync()
    {
        var getAccountResult = await client.GetAsync(Route.GetAllAccounts);
        var accountsJson = await getAccountResult.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AccountApiDto[]>(accountsJson);
    }

    public async Task AddAccountAsync(string name)
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
public class BackendWithSqlite : IDisposable
{
    private SqliteConnection _connection;

    public BackendWithSqlite()
    {
        StartSqliteConnection();

        var appFactory = new SqLiteWebApplicationFactory<Program>(_connection);
        client = appFactory.CreateClient();
    }

    public HttpClient client { get; set; }

    public void Dispose()
    {
        // Sqlite in-memory data is cleared with connection dispose.
        _connection.Dispose();
    }

    private void StartSqliteConnection()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
    }
}