using budget_backend.domain;
using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.Controllers.apiDto;

public record BudgetEntryApiDto(Guid Id, Guid BudgetaryItemId, DateTime Month, double Amount)
{
    public static BudgetEntryApiDto ToApoDtio(BudgetEntry item) =>
        new BudgetEntryApiDto(item.Id, item.BudgetaryItemId, item.Month, item.Amount);
}