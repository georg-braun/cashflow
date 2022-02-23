namespace budget_backend.domain.budget;

public static class BudgetFactory
{
    public static BudgetaryItem Create(string name)
    {
        var id = Guid.NewGuid();
        return new BudgetaryItem(id, name);
    }
    
    public static BudgetChange CreateBudgetChange(Guid budgetaryItemId, double amount, DateOnly today)
    {
        var id = Guid.NewGuid();
        return new BudgetChange(id, budgetaryItemId, today, amount);
    }
    public static Spending CreateSpending(Guid accountId, Guid accountEntryId, Guid budgetaryItemId)
    {
        return new Spending(accountId, accountEntryId, budgetaryItemId);
    }
}