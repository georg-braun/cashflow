using budget_backend.domain.budget;

namespace budget_backend.Controllers.apiDto;

public record BudgetEntryApiDto(Guid Id, Guid BudgetaryItemId, DateTime Month, double Amount)
{
    public static BudgetEntryApiDto ToApiDto(BudgetEntry item) =>
        new BudgetEntryApiDto(item.Id.Id, item.BudgetaryItemId.Id, item.Month, item.Amount);
}