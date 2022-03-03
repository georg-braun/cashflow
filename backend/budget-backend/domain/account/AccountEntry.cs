namespace budget_backend.domain.account;


public class AccountEntry
{
    public AccountEntry(AccountEntryId id, AccountId accountId, double amount, DateOnly date)
    {
        Id = id;
        Amount = amount;
        Date = date;
        AccountId = accountId;
    }

    public AccountEntryId Id { get; }
    public double Amount { get; }
    public DateOnly Date { get; }
    public AccountId AccountId { get; }
}