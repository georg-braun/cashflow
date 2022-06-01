using System.ComponentModel.DataAnnotations;
using budget_backend.application;
using budget_backend.domain;
using budget_backend.domain.budget;

namespace budget_backend.data.dbo;

public class MoneyMovementDbo
{
    [Key] public Guid Id { get; init; }

    public double Amount { get; init; }

    public DateTime Timestamp { get; init; }

    public string Note { get; init; } = string.Empty;

    public Guid CategoryId { get; init; }
    public Guid UserId { get; init; }
}

public static class AccountEntryDtoExtensions
{
    public static MoneyMovementDbo ToDbDto(this MoneyMovement moneyMovement, UserId userId)
    {
        return new MoneyMovementDbo()
        {
            Id = moneyMovement.Id.Id,
            CategoryId = moneyMovement.CategoryId.Id,
            Note = moneyMovement.Note,
            Amount = moneyMovement.Amount,
            Timestamp = moneyMovement.Date,
            UserId = userId.Id
        };
    }

    public static MoneyMovement ToDomain(this MoneyMovementDbo moneyMovementDbo)
    {
        return new MoneyMovement(new MoneyMovementId() {Id = moneyMovementDbo.Id}, moneyMovementDbo.Amount,
            moneyMovementDbo.Timestamp, moneyMovementDbo.Note, CategoryIdFactory.Create(moneyMovementDbo.CategoryId));
    }
}