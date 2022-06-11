using System.ComponentModel.DataAnnotations;
using budget_backend.application;

namespace budget_backend.data.dbo;

public class MoneyMovement
{
    [Key] public Guid Id { get; init; }

    public double Amount { get; init; }

    public DateTime Timestamp { get; init; }

    public string Note { get; init; } = string.Empty;

    public Guid CategoryId { get; init; }
    public Guid UserId { get; init; }
}

public class MoneyMovementId
{
    public Guid Id { get; init; }
}

public static class MoneyMovementIdFactory
{
    public static MoneyMovementId Create(Guid id)
    {
        return new() {Id = id};
    }

    public static MoneyMovementId CreateNew()
    {
        return Create(Guid.NewGuid());
    }
}

public static class MoneyMovementFactory
{
    public static MoneyMovement Create(double amount, DateTime timestamp, string note, CategoryId categoryId, UserId userId)
    {
        return new MoneyMovement
        {
            Id = MoneyMovementIdFactory.CreateNew().Id,
            Amount = amount,
            Timestamp = timestamp,
            Note = note,
            CategoryId = categoryId.Id,
            UserId = userId.Id
        };
    }
}