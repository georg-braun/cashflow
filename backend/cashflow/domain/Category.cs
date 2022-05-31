namespace budget_backend.domain.budget;

public class Category
{
    public Category(CategoryId id, string name)
    {
        Id = id;
        Name = name;
    }

    public CategoryId Id { get; }

    public string Name { get; }
}

public class CategoryId
{
    public Guid Id { get; init; }
}

public static class CategoryIdFactory
{
    public static CategoryId Create(Guid id)
    {
        return new() {Id = id};
    }

    public static CategoryId CreateNew()
    {
        return Create(Guid.NewGuid());
    }
}