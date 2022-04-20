using budget_backend.domain.budget;

namespace budget_backend.domain.account;


public class AccountEntry
{
    public AccountEntry(AccountEntryId id, AccountId accountId, double amount, DateOnly date,
        BudgetaryItemId budgetaryItemId)
    {
        Id = id;
        Amount = amount;
        Date = date;
        AccountId = accountId;
        BudgetaryItemId = budgetaryItemId;
    }

    public AccountEntryId Id { get; }
    public double Amount { get; }
    public DateOnly Date { get; }
    public AccountId AccountId { get; }
    public BudgetaryItemId BudgetaryItemId { get; }
}

public class AccountEntryId
{
    public Guid Id { get; init; }
}