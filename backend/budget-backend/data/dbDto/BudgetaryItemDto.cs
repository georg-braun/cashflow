using budget_backend.application;
using budget_backend.domain.budget;

namespace budget_backend.data.dbDto;

public class BudgetaryItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    
    public Guid UserId { get; init; }
}

public static class BudgetaryItemDtoExtensions
{
    public static BudgetaryItemDto ToDbDto(this BudgetaryItem budgetaryItem, UserId userId)
    {
        return new BudgetaryItemDto()
        {
            Id = budgetaryItem.Id.Id,
            Name =budgetaryItem.Name,
            UserId = userId.Id
        };
    }

    public static BudgetaryItem ToDomain(this BudgetaryItemDto budgetaryItemDto)
    {
        return new BudgetaryItem(BudgetaryItemIdFactory.Create(budgetaryItemDto.Id), budgetaryItemDto.Name);
    }
}