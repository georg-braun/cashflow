using budget_backend.domain.budget;

namespace budget_backend.Controllers.apiDto;

public record BudgetaryItemDto(Guid Id, string Name);

public static class BudgetaryItemDtoExtensions
{
    public static BudgetaryItemDto ToApiDto(this BudgetaryItem item) => new(item.Id.Id, item.Name);
}