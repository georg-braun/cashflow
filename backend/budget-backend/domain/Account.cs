namespace budget_backend.domain;

public class Account
{
    public Account(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }

    public string Name { get; }

    public double Balance => Transactions.Sum(_ => _.Amount);

    private List<Transaction> Transactions { get; } = new();

  
    public void AddTransaction(DateOnly timestamp, double amount)
    {
        var transaction = TransactionFactory.Create(timestamp, amount, this);
        Transactions.Add(transaction);
    }

    public void DeleteTransaction(Guid id)
    {
        Transactions.RemoveAll(_ => _.Id.Equals(id));
    }

    public IEnumerable<Transaction> GetTransactions()
    {
        return Transactions.ToList();
    }
}