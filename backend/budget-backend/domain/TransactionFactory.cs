namespace budget_backend.domain;

public static class TransactionFactory
{
    public static Transaction Create(DateTime timestamp, double amount)
    {
        var id = new Guid();
        return new Transaction(id, timestamp, amount);
    }

    
}