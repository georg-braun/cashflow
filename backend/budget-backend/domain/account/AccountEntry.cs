namespace budget_backend.domain.account;


public class AccountEntry
{
    public AccountEntry(AccountEntryId id, Guid accountId, double amount, DateOnly date)
    {
        Id = id;
        Amount = amount;
        Date = date;
        AccountId = accountId;
    }

    public AccountEntryId Id { get; }
    public double Amount { get; }
    public DateOnly Date { get; }
    public Guid AccountId { get; }
}