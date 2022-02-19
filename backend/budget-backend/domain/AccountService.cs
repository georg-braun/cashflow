using budget_backend.data;
namespace budget_backend.domain;

public interface IAccountService
{
    Task AddNewAccount(Account account);
    Task<Account> GetAccountAsync(string accountName);
}

public class AccountService : IAccountService
{
    private readonly DataContext _dataContext;

    public AccountService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddNewAccount(Account account)
    {
        await _dataContext.AddAccountAsync(account);
    }

    public Task<Account> GetAccountAsync(string accountName)
    {
        return _dataContext.GetAccountAsync(accountName);
        
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

    public static AccountEntry CreateEntry(Account account, double amount, DateOnly timestamp)
    {
        var id = Guid.NewGuid();
        return new AccountEntry(id, account, amount, timestamp);
    }
}