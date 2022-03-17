namespace budget_backend.domain.budget;

public class BudgetChange
{
    public BudgetChange(BudgetChangeId id, BudgetaryItemId budgetaryItemId, DateOnly date, double amount)
    {
        Id = id;
        BudgetaryItemId = budgetaryItemId;
        Date = date;
        Amount = amount;
    }

    public BudgetChangeId Id { get; }

    public BudgetaryItemId BudgetaryItemId { get; }
    
    public DateOnly Date { get; }
    
    public double Amount { get; }
}


public class BudgetChangeId
{
    public Guid Id { get; init; }
}

public static class BudgetChangeIdFactory
{
    public static BudgetChangeId Create(Guid id) => new BudgetChangeId {Id = id};
}