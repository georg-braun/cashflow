using budget_backend.domain.account;

namespace budget_backend.domain.budget;

public static class BudgetFactory
{
    public static BudgetaryItem Create(string name)
    {
        var id = BudgetaryItemIdFactory.Create(Guid.NewGuid());
        return new BudgetaryItem(id, name);
    }
    
    public static BudgetEntry CreateBudgetEntry(BudgetaryItemId budgetaryItemId, double amount, DateTime today)
    {
        var id = BudgetEntryIdFactory.Create(Guid.NewGuid());
        return new BudgetEntry(id, budgetaryItemId, today, amount);
    }
    public static Spending CreateSpending(AccountId accountId, AccountEntryId accountEntryId, BudgetaryItemId budgetaryItemId)
    {
        return new Spending(accountId, accountEntryId, budgetaryItemId);
    }
}