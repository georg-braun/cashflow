namespace budget_backend.domain;

public static class TransactionFactory
{
    public static Transaction Create(DateOnly timestamp, double amount, Account account)
    {
        var id = Guid.NewGuid();
        return new Transaction(id, timestamp, amount, account);
    }   
    
}