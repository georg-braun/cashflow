
using budget_backend.data;
using budget_backend.data.dbo;

namespace budget_backend.Controllers.apiDto;

public record ChangesContainerDto
{
    public IList<MoneyMovementDto> MoneyMovements { get; init; } = new List<MoneyMovementDto>();
    public IList<Guid> DeletedMoneyMovementIds { get; init; } = new List<Guid>();
    public IList<CategoryDto> Categories { get; init; } = new List<CategoryDto>();
    public IList<Guid> DeletedCategoryIds { get; init; } = new List<Guid>();
}

public static class ChangesContainerDtoFactory
{
    private static ChangesContainerDto CreateContainer()
    {
        return new();
    }

    public static ChangesContainerDto Create(MoneyMovement moneyMovement)
    {
        var container = CreateContainer();
        container.MoneyMovements.Add(moneyMovement.ToApiDto());
        return container;
    }

    public static ChangesContainerDto Create(Category category)
    {
        var container = CreateContainer();
        container.Categories.Add(category.ToApiDto());
        return container;
    }

    public static ChangesContainerDto Create(IEnumerable<Category> categories)
    {
        var container = CreateContainer();
        foreach (var category in categories) container.Categories.Add(category.ToApiDto());
        return container;
    }

    public static ChangesContainerDto Create(IEnumerable<MoneyMovement> moneyMovements)
    {
        var container = CreateContainer();
        foreach (var moneyMovement in moneyMovements) container.MoneyMovements.Add(moneyMovement.ToApiDto());
        return container;
    }

    public static ChangesContainerDto Create(IEnumerable<MoneyMovement> moneyMovements,
        IEnumerable<Category> categories)
    {
        var container = CreateContainer();
        foreach (var moneyMovement in moneyMovements) container.MoneyMovements.Add(moneyMovement.ToApiDto());

        foreach (var category in categories) container.Categories.Add(category.ToApiDto());
        return container;
    }

    public static ChangesContainerDto Create(ChangesContainer changesContainer)
    {
        var container = CreateContainer();

        foreach (var (category, changeKind) in changesContainer.Categories)
            if (changeKind == ChangeKind.Created)
                container.Categories.Add(category.ToApiDto());
            else
                container.DeletedCategoryIds.Add(category.Id);

        foreach (var (moneyMovement, changeKind) in changesContainer.MoneyMovements)
            if (changeKind == ChangeKind.Created)
                container.MoneyMovements.Add(moneyMovement.ToApiDto());
            else
                container.DeletedMoneyMovementIds.Add(moneyMovement.Id);
        return container;
    }
}