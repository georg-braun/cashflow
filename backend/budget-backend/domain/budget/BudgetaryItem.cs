namespace budget_backend.domain.budget;

public class BudgetaryItem
{
    public BudgetaryItem(BudgetaryItemId id, string name)
    {
        Id = id;
        Name = name;
    }

    public BudgetaryItemId Id { get; }

    public string Name { get; }
}

public class BudgetaryItemId
{
    public Guid Id { get; init; }
}

public static class BudgetaryItemIdFactory
{
    public static BudgetaryItemId Create(Guid id) => new BudgetaryItemId {Id = id};
}