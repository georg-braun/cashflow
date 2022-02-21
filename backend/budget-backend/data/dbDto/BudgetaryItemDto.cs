using budget_backend.domain.budget;

namespace budget_backend.data.dbDto;

public record BudgetaryItemDto(Guid Id, string Name);

public static class BudgetaryItemDtoExtensions
{
    public static BudgetaryItemDto ToDbDto(this BudgetaryItem budgetaryItem)
    {
        return new BudgetaryItemDto(budgetaryItem.Id, budgetaryItem.Name);
    }

    public static BudgetaryItem ToDomain(this BudgetaryItemDto budgetaryItemDto)
    {
        return new BudgetaryItem(budgetaryItemDto.Id, budgetaryItemDto.Name);
    }
}