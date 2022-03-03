namespace budget_backend.domain.account;

public class AccountId
{
    public Guid Id { get; init; }
}

public static class AccountIdFactory
{
    public static AccountId Create(Guid id) => new AccountId {Id = id};
}