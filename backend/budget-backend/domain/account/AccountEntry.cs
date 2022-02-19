using budget_backend.domain.budget;

namespace budget_backend.domain;


public class AccountEntry
{
    public AccountEntry(Guid id, Account account, double amount, DateOnly timestamp)
    {
        Id = id;
        Amount = amount;
        Timestamp = timestamp;
        Account = account;
    }

    public Guid Id { get; }
    public double Amount { get; }
    public DateOnly Timestamp { get; }
    public Account Account { get; }
}