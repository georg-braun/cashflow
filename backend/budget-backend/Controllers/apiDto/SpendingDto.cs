using budget_backend.domain.budget;

namespace budget_backend.Controllers.apiDto;

public record SpendingDto(Guid AccountId, Guid AccountEntryId, Guid BudgetaryItemId);


public static class SpendingDtoExtensions
{
    public static SpendingDto ToApiDto(this Spending item) => new(item.AccountId.Id, item.AccountEntryId.Id, item.BudgetaryItemId);

}