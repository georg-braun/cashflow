namespace budget_backend.domain.account;


public class AccountEntry
{
    public AccountEntry(Guid id, Guid accountId, double amount, DateOnly date)
    {
        Id = id;
        Amount = amount;
        Date = date;
        AccountId = accountId;
    }

    public Guid Id { get; }
    public double Amount { get; }
    public DateOnly Date { get; }
    public Guid AccountId { get; }
}