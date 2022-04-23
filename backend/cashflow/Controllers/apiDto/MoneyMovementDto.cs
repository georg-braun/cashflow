using budget_backend.domain;

namespace budget_backend.Controllers.apiDto;

public record MoneyMovementDto(Guid Id, double Amount, DateTime Date, string Note, Guid CategoryId);

public static class MoneyMovementDtoExtensions
{
    public static MoneyMovementDto ToApiDto(this MoneyMovement moneyMovement)
    {
        return new MoneyMovementDto(
            moneyMovement.Id.Id,
            moneyMovement.Amount,
            moneyMovement.Date,
            moneyMovement.Note,
            moneyMovement.CategoryId.Id);
    }
}