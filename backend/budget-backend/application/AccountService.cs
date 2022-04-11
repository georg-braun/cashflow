using budget_backend.data;
using budget_backend.domain;
using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.application;

public interface IAccountService
{
    Task<Account> AddNewAccountAsync(UserId userId, string accountName);
    Task<AccountEntry?> AddIncomeAsync(AccountId accountId, double amount, DateOnly timestamp, UserId userId);

    /// <summary>
    ///     Create an account entry (spending) and associate this with a budget.
    /// </summary>
    Task<Spending?> AddSpendingAsync(AccountId accountId, BudgetaryItemId budgetaryItemId, double amount,
        DateOnly timestamp, UserId userId);

    Task<BudgetaryItem> AddBudgetaryItemAsync(string budgetName, UserId userId);

    IEnumerable<BudgetaryItem> GetBudgetaryItems(UserId userId);

    Task<BudgetEntry> AddBudgetEntryAsync(BudgetaryItemId budgetaryItemId, double amount, DateTime month, UserId userId);
    IEnumerable<Account> GetAccounts(UserId userId);
    IEnumerable<AccountEntry> GetAccountEntries(AccountId accountId, UserId userId);
    IEnumerable<BudgetEntry> GetBudgetEntries(BudgetaryItemId budgetaryItemId, UserId userId);
    IEnumerable<Spending> GetSpendings(UserId userId);
    Task<AccountEntry?> GetAccountEntryAsync(AccountEntryId spendingAccountEntryId, UserId userId);
    Task<bool> DeleteAccountAsync(UserId userId, Guid accountId);
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

    public async Task<Account> AddNewAccountAsync(UserId userId, string accountName)
    {
        var account = AccountFactory.Create(accountName);
        await _dataContext.AddAccountAsync(account, userId);
        return account;
    }
    
    public async Task<AccountEntry?> AddIncomeAsync(AccountId accountId, double amount, DateOnly timestamp,
        UserId userId)
    {
        if (amount < 0)
            return null;
        
        var accountEntry = AccountFactory.CreateEntry(accountId, amount, timestamp);
        await _dataContext.AddAccountEntryAsync(accountEntry, userId);
        return accountEntry;
    }

    public async Task<Spending?> AddSpendingAsync(AccountId accountId, BudgetaryItemId budgetaryItemId, double amount,
        DateOnly timestamp, UserId userId)
    {
        // amount has to be negative
        var correctAmount = amount > 0 ? -amount : amount;
        
        var accountEntry = AccountFactory.CreateEntry(accountId, correctAmount, timestamp);
        var spending = BudgetFactory.CreateSpending(accountEntry.AccountId, accountEntry.Id, budgetaryItemId);
        await _dataContext.AddSpendingAsync(accountEntry, spending, userId);
        return spending;
    }
    
    public async Task<BudgetaryItem> AddBudgetaryItemAsync(string budgetName, UserId userId)
    {
        var budgetaryItem = BudgetFactory.Create(budgetName);
        await _dataContext.AddBudgetaryItemAsync(budgetaryItem, userId);
        return budgetaryItem;
    }

    public IEnumerable<BudgetaryItem> GetBudgetaryItems(UserId userId) => _dataContext.GetBudgetaryItems(userId);

    public async Task<BudgetEntry> AddBudgetEntryAsync(BudgetaryItemId budgetaryItemId, double amount, DateTime month,
        UserId userId)
    {
        var typedBudgetaryItemId = budgetaryItemId;
        var budgetEntry = BudgetFactory.CreateBudgetEntry(typedBudgetaryItemId, amount, month);
        await _dataContext.AddBudgetEntryAsync(budgetEntry, userId);
        return budgetEntry;
    }

    public IEnumerable<Account> GetAccounts(UserId userId)
    {
        return _dataContext.GetAccounts(userId);
    }

    public IEnumerable<AccountEntry> GetAccountEntries(AccountId accountId, UserId userId)
    {
        return _dataContext.GetAccountEntries(accountId, userId);
    }

    public IEnumerable<BudgetEntry> GetBudgetEntries(BudgetaryItemId budgetaryItemId, UserId userId) => _dataContext.GetBudgetEntries(budgetaryItemId, userId);
    public IEnumerable<Spending> GetSpendings(UserId userId) => _dataContext.GetSpendings(userId);
    
    public Task<AccountEntry?> GetAccountEntryAsync(AccountEntryId spendingAccountEntryId, UserId userId) => _dataContext.GetAccountEntryAsync(spendingAccountEntryId, userId);

    public Task<bool> DeleteAccountAsync(UserId userId, Guid accountId) =>
        _dataContext.DeleteAccountAsync(accountId, userId);
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