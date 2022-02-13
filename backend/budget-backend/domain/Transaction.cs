namespace budget_backend.domain;

public class Transaction
{
    public Transaction(Guid id, DateTime timestamp, double amount)
    {
        Id = id;
        Amount = amount;
        Timestamp = timestamp;
    }

    public Guid Id { get; init; }

    public double Amount { get; init; }

    public DateTime Timestamp { get; init; }
}