using budget_backend.data;
using budget_backend.domain;
using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.application;

public interface IAccountService
{
    Task<Account> AddNewAccountAsync(Guid userId, string accountName);
    Account? GetAccount(string accountName);
    Task<AccountEntry?> AddIncomeAsync(AccountId accountId, double amount, DateOnly timestamp);

    /// <summary>
    ///     Create an account entry (spending) and associate this with a budget.
    /// </summary>
    Task<Spending?> AddSpendingAsync(AccountId accountId, BudgetaryItemId budgetaryItemId, double amount, DateOnly timestamp);

    Task<BudgetaryItem> AddBudgetaryItemAsync(string budgetName);

    IEnumerable<BudgetaryItem> GetBudgetaryItems();

    Task<BudgetEntry> AddBudgetEntryAsync(BudgetaryItemId budgetaryItemId, double amount, DateTime month);
    IEnumerable<Account> GetAccounts();
    IEnumerable<AccountEntry> GetAccountEntries(AccountId accountId);
    IEnumerable<BudgetEntry> GetBudgetEntries(BudgetaryItemId budgetaryItemId);
    IEnumerable<Spending> GetSpendings();
    Task<AccountEntry?> GetAccountEntryAsync(AccountEntryId spendingAccountEntryId, Guid userId);
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

    public async Task<Account> AddNewAccountAsync(Guid userId, string accountName)
    {
        var account = AccountFactory.Create(accountName);
        await _dataContext.AddAccountAsync(account, userId);
        return account;
    }

    public Account? GetAccount(string accountName)
    {
        return _dataContext.GetAccount(accountName);
    }

    public async Task<AccountEntry?> AddIncomeAsync(AccountId accountId, double amount, DateOnly timestamp)
    {
        if (amount < 0)
            return null;
        
        var accountEntry = AccountFactory.CreateEntry(accountId, amount, timestamp);
        await _dataContext.AddAccountEntryAsync(accountEntry);
        return accountEntry;
    }

    public async Task<Spending?> AddSpendingAsync(AccountId accountId, BudgetaryItemId budgetaryItemId, double amount, DateOnly timestamp)
    {
        if (amount > 0)
            return null;
        
        var accountEntry = AccountFactory.CreateEntry(accountId, amount, timestamp);
        var spending = BudgetFactory.CreateSpending(accountEntry.AccountId, accountEntry.Id, budgetaryItemId);
        await _dataContext.AddSpendingAsync(accountEntry, spending);
        return spending;
    }
    
    public async Task<BudgetaryItem> AddBudgetaryItemAsync(string budgetName)
    {
        var budgetaryItem = BudgetFactory.Create(budgetName);
        await _dataContext.AddBudgetaryItemAsync(budgetaryItem);
        return budgetaryItem;
    }

    public IEnumerable<BudgetaryItem> GetBudgetaryItems() => _dataContext.GetBudgetaryItems();

    public async Task<BudgetEntry> AddBudgetEntryAsync(BudgetaryItemId budgetaryItemId, double amount, DateTime month)
    {
        var typedBudgetaryItemId = budgetaryItemId;
        var budgetEntry = BudgetFactory.CreateBudgetEntry(typedBudgetaryItemId, amount, month);
        await _dataContext.AddBudgetEntryAsync(budgetEntry);
        return budgetEntry;
    }

    public IEnumerable<Account> GetAccounts()
    {
        return _dataContext.GetAccounts();
    }

    public IEnumerable<AccountEntry> GetAccountEntries(AccountId accountId)
    {
        return _dataContext.GetAccountEntries(accountId);
    }

    public IEnumerable<BudgetEntry> GetBudgetEntries(BudgetaryItemId budgetaryItemId) => _dataContext.GetBudgetEntries(budgetaryItemId);
    public IEnumerable<Spending> GetSpendings() => _dataContext.GetSpendings();
    
    public Task<AccountEntry?> GetAccountEntryAsync(AccountEntryId spendingAccountEntryId, Guid userId) => _dataContext.GetAccountEntryAsync(spendingAccountEntryId, userId);
}

public static class AccountFactory
{
    public static Account Create(string name)
    {
        var id = new AccountId {Id = Guid.NewGuid()};
        return Create(id, name);
    }

    public static Account Create(AccountId id, string name)
    {
        return new Account(id, name);
    }

    public static AccountEntry CreateEntry(AccountId accountId, double amount, DateOnly timestamp)
    {
        var id = new AccountEntryId(){Id = Guid.NewGuid()};
        return new AccountEntry(id, accountId, amount, timestamp);
    }

}