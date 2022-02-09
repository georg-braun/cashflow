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
}