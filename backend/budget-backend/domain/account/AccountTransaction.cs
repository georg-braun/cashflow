namespace budget_backend.domain.account;




public class AccountTransaction
{
    public AccountTransaction(Guid id, Guid fromAccountId, Guid fromAccountEntryId, Guid toAccountId, Guid toAccountEntryId)
    {
        Id = id;
        FromAccountId = fromAccountId;
        FromAccountEntryId = fromAccountEntryId;
        ToAccountId = toAccountId;
        ToAccountEntryId = toAccountEntryId;
    }

    public Guid ToAccountEntryId { get; set; }

    public Guid FromAccountEntryId { get; set; }

    public Guid Id { get; }
    public Guid FromAccountId { get; }
    public Guid ToAccountId { get; }
}