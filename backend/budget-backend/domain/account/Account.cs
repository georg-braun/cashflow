using budget_backend.domain.budget;

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
    
    private List<AccountEntry> Entries { get; } = new();

    public double GetBalance()
    {
        return Entries.Sum(_ => _.Amount);
    }

    public AccountEntry AddEntry(double amount, DateOnly timestamp)
    {
        var accountEntry = AccountFactory.CreateEntry(this, amount, timestamp);
        Entries.Add(accountEntry);
        return accountEntry;
    }

    public void AddEntries(IEnumerable<AccountEntry> entries)
    {
        Entries.AddRange(entries);
    }

    public IReadOnlyCollection<AccountEntry> GetEntries() => Entries.ToList();
}