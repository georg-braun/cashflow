using budget_backend.domain.budget;

namespace budget_backend.data.dbDto;

public class BudgetaryItemDto
{
    public BudgetaryItemDto(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; init; }
    public string Name { get; init; }
}

public static class BudgetaryItemDtoExtensions
{
    public static BudgetaryItemDto ToDbDto(this BudgetaryItem budgetaryItem)
    {
        return new BudgetaryItemDto(budgetaryItem.Id.Id, budgetaryItem.Name);
    }

    public static BudgetaryItem ToDomain(this BudgetaryItemDto budgetaryItemDto)
    {
        return new BudgetaryItem(BudgetaryItemIdFactory.Create(budgetaryItemDto.Id), budgetaryItemDto.Name);
    }
}