using budget_backend.domain.budget;

namespace budget_backend.domain;


public class MoneyMovement
{
    public MoneyMovement(MoneyMovementId id, double amount, DateTime date, string note, CategoryId categoryId)
    {
        Id = id;
        Amount = amount;
        Date = date;
        Note = note;
        CategoryId = categoryId;
    }

    public MoneyMovementId Id { get; }
    public double Amount { get; }
    public DateTime Date { get; }
    
    public string Note { get; } = string.Empty;
    public CategoryId CategoryId { get; }
}

public class MoneyMovementId
{
    public Guid Id { get; init; }
}

public static class MoneyMovementIdFactory
{
    public static MoneyMovementId Create(Guid id) => new MoneyMovementId {Id = id};

    public static MoneyMovementId CreateNew() => Create(Guid.NewGuid());
}