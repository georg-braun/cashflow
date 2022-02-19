using budget_backend.domain.budget;

namespace budget_backend.domain;

public static class AccountTransactionFactory
{

    public static AccountTransaction Create(AccountEntry from, AccountEntry to)
    {
        var id = Guid.NewGuid();
        return Create(id, from, to);
    }

    public static AccountTransaction Create(Guid id, AccountEntry from, AccountEntry to)
    {
        return new AccountTransaction(id, from, to);
    }

    // todo: I think this should be a service and not a factory because it modifies the accounts.
    public static AccountTransaction Create(Account from, Account to, double amount, DateOnly timestamp)
    {
        var fromAccountEntry = from.AddEntry(-amount, timestamp);
        var toAccountEntry = to.AddEntry(amount, timestamp);
        return Create(fromAccountEntry, toAccountEntry);
    }
}