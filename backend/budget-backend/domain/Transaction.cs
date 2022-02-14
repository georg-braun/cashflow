namespace budget_backend.domain;

public class Transaction
{
    public Transaction(Guid id, DateOnly timestamp, double amount, Account account)
    {
        Id = id;
        Amount = amount;
        Timestamp = timestamp;
        Account = account;
    }

    public Guid Id { get; init; }

    public double Amount { get; init; }

    public DateOnly Timestamp { get; }
    
    public Account Account { get; }
}