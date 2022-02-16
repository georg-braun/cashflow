using AutoMapper;
using budget_backend.data;

namespace budget_backend.domain;

public interface IAccountService
{
    Task AddAccountAsync(Account account);
    Task<bool> GetAccountAsync(string accountName, out Account? account);
}

public class AccountService : IAccountService
{
    private readonly DataContext _dataContext;

    public AccountService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddAccountAsync(Account account)
    {
        await _dataContext.AddAccountAsync(account);
    }

    public Task<bool> GetAccountAsync(string accountName, out Account? account)
    {
        var result = _dataContext.GetAccountAsync(accountName, out var repoAccount);
        account = repoAccount;
        return result;
    }

    /*
    public bool TryGet(Guid id, out Account account)
    {
        var foundAccount = _accounts.FirstOrDefault(_ => _.Id.Equals(id));
        account = foundAccount ?? GetDefaultAccount();
        return foundAccount != null;
    }

    public bool TryGet(string accountName, out Account account)
    {
        var foundAccount = _accounts.FirstOrDefault(_ => _.Name.Equals(accountName));
        account = foundAccount ?? GetDefaultAccount();
        return foundAccount != null;
    }

    public void Update(Account account)
    {
        Delete(account);
        _accounts.Add(account);
    }

    public void Delete(Account account)
    {
        _accounts.RemoveAll(_ => _.Id.Equals(account.Id));
    }
    */
}

public static class AccountFactory
{
    public static Account Create(string name)
    {
        var id = Guid.NewGuid();
        return new Account(id, name);
    }
}