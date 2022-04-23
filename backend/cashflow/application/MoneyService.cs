using budget_backend.data;
using budget_backend.domain;
using budget_backend.domain.budget;

namespace budget_backend.application;

public interface IMoneyService
{
    Task<ChangesContainer> AddMoneyMovementAsync(double amount, DateTime date, string note,
        CategoryId categoryId, UserId userId);

    Task<ChangesContainer> AddCategoryAsync(string categoryName, UserId userId);
    IEnumerable<Category> GetCategories(UserId userId);
    IEnumerable<MoneyMovement> GetMoneyMovements(UserId userId);
    Task<ChangesContainer> DeleteCategoryAsync(CategoryId categoryId, UserId userId);
    Task<ChangesContainer> DeleteMoneyMovementAsync(MoneyMovementId moneyMovementId, UserId userId);
}

/// <summary>
///     Work with domain objects and shift data between persistence and clients.
/// </summary>
public class MoneyService : IMoneyService
{
    private readonly DataContext _dataContext;

    public MoneyService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }


    public async Task<ChangesContainer> AddMoneyMovementAsync(double amount, DateTime date, string note,
        CategoryId categoryId, UserId userId)
    {
        var moneyMovement = MoneyMovementFactory.Create(amount, date, note, categoryId);
     
        var changes = await _dataContext.AddMoneyMovementAsync(moneyMovement, userId);
        return changes;
    }

    public async Task<ChangesContainer> AddCategoryAsync(string categoryName, UserId userId)
    {
        var category = CategoryFactory.Create(categoryName);
        var changes = await _dataContext.AddCategoryAsync(category, userId);
        return changes;
    }

    public IEnumerable<Category> GetCategories(UserId userId) => _dataContext.GetCategories(userId);


    public IEnumerable<MoneyMovement> GetMoneyMovements(UserId userId) => _dataContext.GetMoneyMovements(userId);


    public Task<ChangesContainer> DeleteCategoryAsync(CategoryId categoryId, UserId userId) => _dataContext.DeleteCategoryAsync(categoryId, userId);

    public Task<ChangesContainer> DeleteMoneyMovementAsync(MoneyMovementId moneyMovementId, UserId userId) => _dataContext.DeleteMoneyMovementAsync(moneyMovementId, userId);
}