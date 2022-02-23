namespace budget_backend.domain.budget;

public class Spending
{
    public Spending(Guid accountId, Guid accountEntryId, Guid budgetaryItemId)
    {
        AccountId = accountId;
        AccountEntryId = accountEntryId;
        BudgetaryItemId = budgetaryItemId;
    }

    public Guid AccountId { get; }
    public Guid AccountEntryId { get; }
    public Guid BudgetaryItemId { get;  }
}