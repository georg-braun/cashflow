using budget_backend.domain.budget;

namespace budget_backend.domain;




public class AccountTransaction
{
    public AccountTransaction(Guid id, AccountEntry from, AccountEntry to)
    {
        Id = id;
        From = from;
        To = to;
    }

    public Guid Id { get; }
    public AccountEntry From { get; }
    public AccountEntry To { get; }
}