namespace budget_backend.domain;


public class Transaction
{
    public Transaction(Guid id, DateTime timestamp, double amount, Account account)
    {
        Id = id;
        Amount = amount;
        Timestamp = timestamp;
        Account = account;
    }

    public Guid Id { get; init; }
    
    public double Amount { get; init; }
    
    public DateTime Timestamp { get; init; }
    
    public Account Account { get; init; }
}