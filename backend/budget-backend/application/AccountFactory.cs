using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.application;

public static class AccountFactory
{
    public static Account Create(string name)
    {
        var id = new AccountId {Id = Guid.NewGuid()};
        return Create(id, name);
    }

    public static Account Create(AccountId id, string name)
    {
        return new Account(id, name);
    }

    public static AccountEntry CreateEntry(AccountId accountId, double amount, DateOnly timestamp,
        BudgetaryItemId budgetaryItemId)
    {
        var id = new AccountEntryId {Id = Guid.NewGuid()};
        return new AccountEntry(id, accountId, amount, timestamp, budgetaryItemId);
    }
}