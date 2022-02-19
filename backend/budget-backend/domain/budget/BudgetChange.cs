namespace budget_backend.domain.budget;

public class BudgetChange
{
    public BudgetChange(Guid id, BudgetaryItem budgetaryItem, DateOnly timestamp, double amount)
    {
        Id = id;
        BudgetaryItem = budgetaryItem;
        Timestamp = timestamp;
        Amount = amount;
    }

    public Guid Id { get; }

    public BudgetaryItem BudgetaryItem { get; }
    
    public DateOnly Timestamp { get; }
    
    public double Amount { get; }
}