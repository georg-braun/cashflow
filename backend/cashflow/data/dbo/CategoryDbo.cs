using budget_backend.application;
using budget_backend.domain.budget;

namespace budget_backend.data.dbo;

public class CategoryDbo
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid UserId { get; init; }
}

public static class CategoryDtoExtensions
{
    public static CategoryDbo ToDbDto(this Category category, UserId userId)
    {
        return new CategoryDbo()
        {
            Id = category.Id.Id,
            Name =category.Name,
            UserId = userId.Id
        };
    }

    public static Category ToDomain(this CategoryDbo categoryDbo)
    {
        return new Category(CategoryIdFactory.Create(categoryDbo.Id), categoryDbo.Name);
    }
}