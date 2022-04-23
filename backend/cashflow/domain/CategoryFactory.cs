using budget_backend.domain.budget;

namespace budget_backend.domain;

public static class CategoryFactory
{
    public static Category Create(string name)
    {
        return new Category(CategoryIdFactory.CreateNew(), name);
    }
}