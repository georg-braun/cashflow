using budget_backend.domain.account;

namespace budget_backend.domain.budget;

public class Spending
{
    public Spending(AccountId accountId, AccountEntryId accountEntryId, Guid budgetaryItemId)
    {
        AccountId = accountId;
        AccountEntryId = accountEntryId;
        BudgetaryItemId = budgetaryItemId;
    }

    public AccountId AccountId { get; }
    public AccountEntryId AccountEntryId { get; }
    public Guid BudgetaryItemId { get;  }
}