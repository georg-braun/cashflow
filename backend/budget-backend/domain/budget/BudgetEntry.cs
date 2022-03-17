namespace budget_backend.domain.budget;

public class BudgetEntry
{
    public BudgetEntry(BudgetEntryId id, BudgetaryItemId budgetaryItemId, DateTime month, double amount)
    {
        Id = id;
        BudgetaryItemId = budgetaryItemId;
        Month = month;
        Amount = amount;
    }

    public BudgetEntryId Id { get; }

    public BudgetaryItemId BudgetaryItemId { get; }
    
    public DateTime Month { get; }
    
    public double Amount { get; }
}


public class BudgetEntryId
{
    public Guid Id { get; init; }
}

public static class BudgetEntryIdFactory
{
    public static BudgetEntryId Create(Guid id) => new BudgetEntryId() {Id = id};
}