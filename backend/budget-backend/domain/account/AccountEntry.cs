namespace budget_backend.domain.account;


public class AccountEntry
{
    public AccountEntry(Guid id, Guid accountId, double amount, DateOnly timestamp)
    {
        Id = id;
        Amount = amount;
        Timestamp = timestamp;
        AccountId = accountId;
    }

    public Guid Id { get; }
    public double Amount { get; }
    public DateOnly Timestamp { get; }
    public Guid AccountId { get; }
}