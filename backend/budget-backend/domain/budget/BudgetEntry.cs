namespace budget_backend.domain.budget;

public class BudgetEntry
{
    public BudgetEntry(Guid id, Guid budgetaryItemId, DateTime month, double amount)
    {
        Id = id;
        BudgetaryItemId = budgetaryItemId;
        Month = month;
        Amount = amount;
    }

    public Guid Id { get; }

    public Guid BudgetaryItemId { get; }
    
    public DateTime Month { get; }
    
    public double Amount { get; }
}