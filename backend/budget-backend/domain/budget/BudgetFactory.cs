namespace budget_backend.domain.budget;

public static class BudgetFactory
{
    public static BudgetaryItem Create(string name)
    {
        var id = Guid.NewGuid();
        return new BudgetaryItem(id, name);
    }

    public static BudgetChange CreateBudgetChange(BudgetaryItem budgetaryItem, double amount, DateOnly today)
    {
        var id = Guid.NewGuid();
        return new BudgetChange(id, budgetaryItem, today, amount);
    }
}