using budget_backend.data;
using budget_backend.domain;
using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.application;

public interface IAccountService
{
    Task<Account> AddNewAccountAsync(string accountName);
    Task<Account> GetAccountAsync(string accountName);
    Task<AccountEntry> AddIncomeAsync(Guid accountId, double amount, DateOnly timestamp);

    /// <summary>
    ///     Create an account entry (spending) and associate this with a budget.
    /// </summary>
    Task<AccountEntry> AddSpendingAsync(Guid accountId, Guid budgetaryItemId, double amount, DateOnly timestamp);

    Task<BudgetaryItem> AddBudgetaryItemAsync(string budgetName);

    IEnumerable<BudgetaryItem> GetBudgetaryItems();

    Task<BudgetChange> AddBudgetChangeAsync(Guid budgetaryItemId, double amount, DateOnly timestamp);
    Task DeleteBudgetChangeAsync(Guid budgetChangeId);
    IEnumerable<Account> GetAccounts();
    IEnumerable<AccountEntry> GetAccountEntries(Guid accountId);
}

/// <summary>
///     Work with domain objects and shift data between persistence and clients.
/// </summary>
public class AccountService : IAccountService
{
    private readonly DataContext _dataContext;

    public AccountService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Account> AddNewAccountAsync(string accountName)
    {
        var account = AccountFactory.Create(accountName);
        await _dataContext.AddAccountAsync(account);
        return account;
    }

    public Task<Account> GetAccountAsync(string accountName)
    {
        return _dataContext.GetAccount(accountName);
    }

    public async Task<AccountEntry> AddIncomeAsync(Guid accountId, double amount, DateOnly timestamp)
    {
        if (amount < 0)
            return null;
        
        var accountEntry = AccountFactory.CreateEntry(accountId, amount, timestamp);
        await _dataContext.AddAccountEntryAsync(accountEntry);
        return accountEntry;
    }

    public async Task<AccountEntry> AddSpendingAsync(Guid accountId, Guid budgetaryItemId, double amount, DateOnly timestamp)
    {
        if (amount > 0)
            return null;
        
        var accountEntry = AccountFactory.CreateEntry(accountId, amount, timestamp);
        var spending = BudgetFactory.CreateSpending(accountEntry.AccountId, accountEntry.Id, budgetaryItemId);
        await _dataContext.AddSpendingAsync(accountEntry, spending);
        return accountEntry;
    }
    
    public async Task<BudgetaryItem> AddBudgetaryItemAsync(string budgetName)
    {
        var budgetaryItem = BudgetFactory.Create(budgetName);
        await _dataContext.AddBudgetaryItemAsync(budgetaryItem);
        return budgetaryItem;
    }

    public IEnumerable<BudgetaryItem> GetBudgetaryItems() => _dataContext.GetBudgetaryItems();

    public async Task<BudgetChange> AddBudgetChangeAsync(Guid budgetaryItemId, double amount, DateOnly timestamp)
    {
        var budgetChange = BudgetFactory.CreateBudgetChange(budgetaryItemId, amount, timestamp);
        await _dataContext.AddBudgetChangeAsync(budgetChange);
        return budgetChange;
    }

    public async Task DeleteBudgetChangeAsync(Guid budgetChangeId)
    {
        await _dataContext.DeleteBudgetChangeAsync(budgetChangeId);
    }

    public IEnumerable<Account> GetAccounts()
    {
        return _dataContext.GetAccounts();
    }

    public IEnumerable<AccountEntry> GetAccountEntries(Guid accountId)
    {
        return _dataContext.GetAccountEntries(accountId);
    }
}

public static class AccountFactory
{
    public static Account Create(string name)
    {
        var id = Guid.NewGuid();
        return Create(id, name);
    }

    public static Account Create(Guid id, string name)
    {
        return new Account(id, name);
    }

    public static AccountEntry CreateEntry(Guid accountId, double amount, DateOnly timestamp)
    {
        var id = Guid.NewGuid();
        return new AccountEntry(id, accountId, amount, timestamp);
    }

}