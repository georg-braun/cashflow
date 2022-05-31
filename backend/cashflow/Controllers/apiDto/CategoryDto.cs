using budget_backend.domain.budget;

namespace budget_backend.Controllers.apiDto;

public record CategoryDto(Guid Id, string Name);

public static class BudgetaryItemDtoExtensions
{
    public static CategoryDto ToApiDto(this Category item)
    {
        return new(item.Id.Id, item.Name);
    }
}