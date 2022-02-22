namespace budget_backend.domain.budget;

public class BudgetChange
{
    public BudgetChange(Guid id, Guid budgetaryItemId, DateOnly date, double amount)
    {
        Id = id;
        BudgetaryItemId = budgetaryItemId;
        Date = date;
        Amount = amount;
    }

    public Guid Id { get; }

    public Guid BudgetaryItemId { get; }
    
    public DateOnly Date { get; }
    
    public double Amount { get; }
}