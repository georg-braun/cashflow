using budget_backend.data.dbo;

namespace budget_backend.Controllers.apiDto;

public record MoneyMovementDto(Guid Id, double Amount, DateTime Date, string Note, Guid CategoryId);

public static class MoneyMovementDtoExtensions
{
    public static MoneyMovementDto ToApiDto(this MoneyMovement moneyMovement)
    {
        return new MoneyMovementDto(
            moneyMovement.Id,
            moneyMovement.Amount,
            moneyMovement.Timestamp,
            moneyMovement.Note,
            moneyMovement.CategoryId);
    }
}