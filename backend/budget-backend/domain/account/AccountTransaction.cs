namespace budget_backend.domain.account;




public class AccountTransaction
{
    public AccountTransaction(AccountTransactionId id, AccountId fromAccountId, AccountEntryId fromAccountEntryId, AccountId toAccountId, AccountEntryId toAccountEntryId)
    {
        Id = id;
        FromAccountId = fromAccountId;
        FromAccountEntryId = fromAccountEntryId;
        ToAccountId = toAccountId;
        ToAccountEntryId = toAccountEntryId;
    }

    public AccountEntryId ToAccountEntryId { get; set; }

    public AccountEntryId FromAccountEntryId { get; set; }

    public AccountTransactionId Id { get; }
    public AccountId FromAccountId { get; }
    public AccountId ToAccountId { get; }
}

public class AccountTransactionId
{
    public Guid Id { get; init; }
}

public static class AccountTransactionIdFactory
{
    public static AccountTransactionId Create(Guid id) => new AccountTransactionId {Id = id};
}