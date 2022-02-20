namespace budget_backend.domain.budget;

public class BudgetChange
{
    public BudgetChange(Guid id, Guid budgetaryItemId, DateOnly timestamp, double amount)
    {
        Id = id;
        BudgetaryItemId = budgetaryItemId;
        Timestamp = timestamp;
        Amount = amount;
    }

    public Guid Id { get; }

    public Guid BudgetaryItemId { get; }
    
    public DateOnly Timestamp { get; }
    
    public double Amount { get; }
}