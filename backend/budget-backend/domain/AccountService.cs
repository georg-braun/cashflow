namespace budget_backend.domain;

public class AccountService
{
    private const string cDefaultAccountName = "default account";
    private List<Account> _accounts = new(){AccountFactory.Create(cDefaultAccountName)};
    
    public void Add(string name)
    {
        var account = AccountFactory.Create(name);
        _accounts.Add(account);
    }
    
    private Account GetDefaultAccount() {
        TryGet(cDefaultAccountName, out var defaultAccount);
        return defaultAccount;
    }

    public bool TryGet(Guid id, out Account account)
    {
        var foundAccount =_accounts.FirstOrDefault(_ => _.Id.Equals(id)) ;
        account = foundAccount ?? GetDefaultAccount();
        return foundAccount != null;
    }
    
    public bool TryGet(string accountName, out Account account)
    {
        var foundAccount =_accounts.FirstOrDefault(_ => _.Name.Equals(accountName));
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
}

public static class AccountFactory
{
    public static Account Create(string name)
    {
        var id = new Guid();
        return new Account(id, name);
    }
}

