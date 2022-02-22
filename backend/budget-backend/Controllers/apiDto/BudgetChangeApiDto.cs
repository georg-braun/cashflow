using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.Controllers.apiDto;

public record BudgetChangeApiDto(Guid Id, Guid BudgetaryItemId, double Amount, DateTime Date);

public static class BudgetChangesApiDtoExtensions
{
    public static BudgetChangeApiDto ToApiDto(this BudgetChange item) => new(item.Id, item.BudgetaryItemId, item.Amount, item.Date.ToDateTime(TimeOnly.MinValue));
}