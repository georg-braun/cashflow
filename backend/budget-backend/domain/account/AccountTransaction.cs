namespace budget_backend.domain.account;




public class AccountTransaction
{
    public AccountTransaction(Guid id, Guid fromAccountId, AccountEntryId fromAccountEntryId, Guid toAccountId, AccountEntryId toAccountEntryId)
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
    public Guid FromAccountId { get; }
    public Guid ToAccountId { get; }
}