using budget_backend.data;
using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.application;

public interface IAccountService
{
    Task<Account> AddNewAccountAsync(UserId userId, string accountName);
    Task<AccountEntry?> AddAccountEntryAsync(AccountId accountId, double amount, DateOnly timestamp, UserId userId,
        BudgetaryItemId budgetaryItemId);
    
    Task<BudgetaryItem> AddBudgetaryItemAsync(string budgetName, UserId userId);

    IEnumerable<BudgetaryItem> GetBudgetaryItems(UserId userId);

    Task<BudgetEntry> AddBudgetEntryAsync(BudgetaryItemId budgetaryItemId, double amount, DateTime month,
        UserId userId);

    IEnumerable<Account> GetAccounts(UserId userId);
    IEnumerable<AccountEntry> GetAccountEntries(AccountId accountId, UserId userId);
    IEnumerable<BudgetEntry> GetBudgetEntries(BudgetaryItemId budgetaryItemId, UserId userId);
    Task<AccountEntry?> GetAccountEntryAsync(AccountEntryId spendingAccountEntryId, UserId userId);
    Task<bool> DeleteAccountAsync(UserId userId, Guid accountId);
    Task<bool> DeleteAccountEntryAsync(UserId userId, Guid accountEntryId);
    Task<bool> DeleteBudgetaryItemAsync(Guid budgetaryItemId, UserId userId);
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

    public async Task<AccountEntry?> AddAccountEntryAsync(AccountId accountId, double amount, DateOnly timestamp,
        UserId userId, BudgetaryItemId budgetaryItemId)
    {

        var accountEntry = AccountFactory.CreateEntry(accountId, amount, timestamp, budgetaryItemId);
        await _dataContext.AddAccountEntryAsync(accountEntry, userId);
        return accountEntry;
    }

    public async Task<BudgetaryItem> AddBudgetaryItemAsync(string budgetName, UserId userId)
    {
        var budgetaryItem = BudgetFactory.Create(budgetName);
        await _dataContext.AddBudgetaryItemAsync(budgetaryItem, userId);
        return budgetaryItem;
    }

    public IEnumerable<BudgetaryItem> GetBudgetaryItems(UserId userId)
    {
        return _dataContext.GetBudgetaryItems(userId);
    }

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

    public IEnumerable<BudgetEntry> GetBudgetEntries(BudgetaryItemId budgetaryItemId, UserId userId)
    {
        return _dataContext.GetBudgetEntries(budgetaryItemId, userId);
    }
    
    public Task<AccountEntry?> GetAccountEntryAsync(AccountEntryId spendingAccountEntryId, UserId userId)
    {
        return _dataContext.GetAccountEntryAsync(spendingAccountEntryId, userId);
    }

    public Task<bool> DeleteAccountAsync(UserId userId, Guid accountId)
    {
        return _dataContext.DeleteAccountAsync(accountId, userId);
    }
    
    public Task<bool> DeleteBudgetaryItemAsync(Guid budgetaryItemId, UserId userId)
    {
        return _dataContext.DeleteBudgetaryItemAsync(budgetaryItemId, userId);
    }

    public Task<bool> DeleteAccountEntryAsync(UserId userId, Guid accountEntryId)
    {
        return _dataContext.DeleteAccountEntryAsync(accountEntryId, userId);
    }
}