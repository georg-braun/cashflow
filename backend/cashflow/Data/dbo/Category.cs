using budget_backend.application;

namespace budget_backend.data.dbo;

public class Category
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid UserId { get; init; }
}

public static class CategoryFactory
{
    public static Category Create(string name, UserId userId)
    {
        return new Category
        {
            Id = CategoryIdFactory.CreateNew().Id,
            Name = name,
            UserId = userId.Id
        };
    }

    
}

public class CategoryId
{
    public Guid Id { get; init; }
}

static class CategoryIdFactory
{
    public static CategoryId Create(Guid id)
    {
        return new CategoryId {Id = id};
    }

    public static CategoryId CreateNew()
    {
        return Create(Guid.NewGuid());
    }
}