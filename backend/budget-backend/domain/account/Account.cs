namespace budget_backend.domain.account;


public class Account
{
    public Account(AccountId id, string name)
    {
        Id = id;
        Name = name;
    }

    public AccountId Id { get; }

    public string Name { get; }
}