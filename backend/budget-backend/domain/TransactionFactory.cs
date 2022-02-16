namespace budget_backend.domain;

public static class TransactionFactory
{
    public static Transaction Create(DateOnly timestamp, double amount, Account account)
    {
        var id = Guid.NewGuid();
        return Create(id, timestamp, amount, account);
    }   
    
    public static Transaction Create(Guid id, DateOnly timestamp, double amount, Account account)
    {
        return new Transaction(id, timestamp, amount, account);
    }   
    
}