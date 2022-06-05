

using budget_backend.data.dbo;

namespace budget_backend.Controllers.apiDto;

public record CategoryDto(Guid Id, string Name);

public static class BudgetaryItemDtoExtensions
{
    public static CategoryDto ToApiDto(this Category item)
    {
        return new(item.Id, item.Name);
    }
}