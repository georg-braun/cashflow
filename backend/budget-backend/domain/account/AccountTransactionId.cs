namespace budget_backend.domain.account;

public class AccountTransactionId
{
    public Guid Id { get; init; }
}

public static class AccountTransactionIdFactory
{
    public static AccountTransactionId Create(Guid id) => new AccountTransactionId {Id = id};
}