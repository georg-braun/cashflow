namespace budget_backend.domain;

public class AccountService
{
    private List<Account> _accounts = new();

    private static Account FallbackAccount = AccountFactory.Create("Fallback Account");
    
    public void AddAccount(string name)
    {
        var account = AccountFactory.Create(name);
        _accounts.Add(account);
    }

    public bool TryGetAccount(Guid id, out Account account)
    {
        var foundAccount =_accounts.FirstOrDefault(_ => _.Id.Equals(id));
        account = foundAccount ?? FallbackAccount;
        return foundAccount != null;
    }
    
    public bool TryGetAccount(string accountName, out Account account)
    {
        var foundAccount =_accounts.FirstOrDefault(_ => _.Name.Equals(accountName));
        account = foundAccount ?? FallbackAccount;
        return foundAccount != null;
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