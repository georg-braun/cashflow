using budget_backend.application;
using budget_backend.data.dbo;
using budget_backend.domain;
using budget_backend.domain.budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace budget_backend.data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }
    
    private DbSet<UserDbo> Users { get; set; } = null!;
    
    private DbSet<MoneyMovementDbo> MoneyMovements { get; set; } = null!;

    private DbSet<CategoryDbo> Categories { get; set; } = null!;


    private ChangesContainer CreateChangesContainer()
    {
        return new ChangesContainer();
    }


    public async Task<ChangesContainer> AddMoneyMovementAsync(MoneyMovement moneyMovement, UserId userId)
    {
        var moneyMovementDto = moneyMovement.ToDbDto(userId);
        await MoneyMovements.AddAsync(moneyMovementDto);
        await SaveChangesAsync();

        var changesContainer = CreateChangesContainer();
        changesContainer.MoneyMovements.Add((moneyMovement, ChangeKind.Created));
        return changesContainer;
    }

    public async Task<ChangesContainer> AddCategoryAsync(Category category, UserId userId)
    {
        var categoryDto = category.ToDbDto(userId);
        await Categories.AddAsync(categoryDto);
        await SaveChangesAsync();

        var changesContainer = CreateChangesContainer();
        changesContainer.Categories.Add((category, ChangeKind.Created));
        return changesContainer;
    }


    public IEnumerable<Category> GetCategories(UserId userId) => Categories.Where(_ => _.UserId.Equals(userId.Id)).Select(_ => _.ToDomain());

    
    public IEnumerable<MoneyMovement> GetMoneyMovements(UserId userId)
    {
        var moneyMovementDtos = MoneyMovements.Where(_ => _.UserId.Equals(userId.Id));
        return moneyMovementDtos.Select(_ => _.ToDomain());
    }

    public async Task<MoneyMovement?> GetMoneyMovementAsync(MoneyMovementId moneyMovementId, UserId userId)
    {
        var moneyMovementDto = await MoneyMovements.SingleOrDefaultAsync(_ => _.Id.Equals(moneyMovementId.Id));
        if (moneyMovementDto != null && moneyMovementDto.UserId.Equals(userId.Id))
            return moneyMovementDto.ToDomain();
        return null;
    }

    public UserId GetUserIdByAuthProviderId(string authProviderId)
    {
        var hFoundUserId = Users.FirstOrDefault(_ => _.AuthProviderId.Equals(authProviderId))?.Id;
        if (hFoundUserId is null || hFoundUserId.Value.Equals(Guid.Empty))
            return UserId.Empty;
        return UserId.New(hFoundUserId.Value);
    }

    public async Task<UserId> AddUserAsync(string authProviderId)
    {
        var userId = GetUserIdByAuthProviderId(authProviderId);

        if (userId.IsValid)
            return userId;
        
        var addedUserEntity = await Users.AddAsync(new UserDbo(Guid.NewGuid(), authProviderId, DateTime.UtcNow));
        return UserId.New(addedUserEntity.Entity.Id);
    }
    
    public async Task<ChangesContainer> DeleteMoneyMovementAsync(MoneyMovementId moneyMovementId, UserId userId)
    {
        var changesContainer = CreateChangesContainer();
        
        var moneyMovement = await MoneyMovements.FirstOrDefaultAsync(_ => _.Id.Equals(moneyMovementId.Id) && _.UserId.Equals(userId.Id));
        if (moneyMovement is null)
            return changesContainer;
        MoneyMovements.Remove(moneyMovement);
         
        await SaveChangesAsync();
        
        changesContainer.MoneyMovements.Add((moneyMovement.ToDomain(), ChangeKind.Deleted));
        return changesContainer;
    }

    public async Task<ChangesContainer> DeleteCategoryAsync(CategoryId categoryId, UserId userId)
    {
        var changesContainer = CreateChangesContainer();

        var category = await Categories.FirstOrDefaultAsync(_ => _.Id.Equals(categoryId.Id) && _.UserId.Equals(userId.Id));
        if (category is null)
            return changesContainer;

        // if an money movement entry is still associated with this category, it's not possible to delete the budget.
        var categoryMoneyMovements = MoneyMovements.FirstOrDefault(_ => _.CategoryId.Equals(categoryId.Id));
        if (categoryMoneyMovements != null)
            return changesContainer;
        
        Categories.Remove(category);
        await SaveChangesAsync();
        
        changesContainer.Categories.Add((category.ToDomain(), ChangeKind.Deleted));
        return changesContainer;
    }
}


public enum ChangeKind
{
    Created,
    Updated,
    Deleted
}

public class ChangesContainer
{
    public IList<(Category, ChangeKind)> Categories = new List<(Category, ChangeKind)>();
    public IList<(MoneyMovement, ChangeKind)> MoneyMovements = new List<(MoneyMovement, ChangeKind)>();
}