namespace budget_backend.domain.account;




public class AccountTransaction
{
    public AccountTransaction(Guid id, AccountId fromAccountId, AccountEntryId fromAccountEntryId, AccountId toAccountId, AccountEntryId toAccountEntryId)
    {
        Id = id;
        FromAccountId = fromAccountId;
        FromAccountEntryId = fromAccountEntryId;
        ToAccountId = toAccountId;
        ToAccountEntryId = toAccountEntryId;
    }

    public AccountEntryId ToAccountEntryId { get; set; }

    public AccountEntryId FromAccountEntryId { get; set; }

    public Guid Id { get; }
    public AccountId FromAccountId { get; }
    public AccountId ToAccountId { get; }
}