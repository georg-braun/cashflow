using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.domain;

public static class AccountTransactionFactory
{

    public static AccountTransaction Create(AccountEntry from, AccountEntry to)
    {
        var id = Guid.NewGuid();
        return Create(id, from, to);
    }

    public static AccountTransaction Create(Guid id, AccountEntry fromEntry, AccountEntry toEntry)
    {
        return new AccountTransaction(id, fromEntry.AccountId, fromEntry.Id, toEntry.AccountId, toEntry.Id);
    }


}