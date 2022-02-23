using budget_backend.domain;
using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.Controllers.apiDto;

public record SpendingDto(Guid AccountId, Guid AccountEntryId, Guid BudgetaryItemId);


public static class SpendingDtoExtensions
{
    public static SpendingDto ToApiDto(this Spending item) => new(item.AccountId, item.AccountEntryId, item.BudgetaryItemId);

}