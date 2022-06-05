using budget_backend.application;
using budget_backend.data.dbo;
using Microsoft.EntityFrameworkCore;

namespace budget_backend.data;

public class CashflowDataContext : DbContext
{
    public CashflowDataContext(DbContextOptions options) : base(options)
    {
    }

    private DbSet<User> Users { get; set; } = null!;
    private DbSet<MoneyMovement> MoneyMovements { get; set; } = null!;
    private DbSet<Category> Categories { get; set; } = null!;
    private DbSet<Template> Templates { get; set; } = null!;


    private ChangesContainer CreateChangesContainer()
    {
        return new ChangesContainer();
    }


    public async Task<ChangesContainer> AddMoneyMovementAsync(MoneyMovement moneyMovement)
    {
        await MoneyMovements.AddAsync(moneyMovement);
        await SaveChangesAsync();
    
        var changesContainer = CreateChangesContainer();
        changesContainer.MoneyMovements.Add((moneyMovement, ChangeKind.Created));
        return changesContainer;
    }

    public async Task<ChangesContainer> AddCategoryAsync(Category category)
    {
        await Categories.AddAsync(category);
        await SaveChangesAsync();

        var changesContainer = CreateChangesContainer();
        changesContainer.Categories.Add((category, ChangeKind.Created));
        return changesContainer;
    }


    public Task<List<Category>> GetCategories(UserId userId)
    {
        return Task.FromResult(Categories.Where(_ => _.UserId.Equals(userId.Id)).ToList());
    }
    
    
    public Task<List<MoneyMovement>> GetMoneyMovements(UserId userId)
    {
        var moneyMovementDtos = MoneyMovements.Where(_ => _.UserId.Equals(userId.Id));
        return Task.FromResult(moneyMovementDtos.ToList());
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
    
        var addedUserEntity = await Users.AddAsync(new User(Guid.NewGuid(), authProviderId, DateTime.UtcNow));
        return UserId.New(addedUserEntity.Entity.Id);
    }
    
    public async Task<ChangesContainer> DeleteMoneyMovementAsync(MoneyMovementId moneyMovementId, UserId userId)
    {
        var changesContainer = CreateChangesContainer();
    
        var moneyMovement =
            await MoneyMovements.FirstOrDefaultAsync(_ =>
                _.Id.Equals(moneyMovementId.Id) && _.UserId.Equals(userId.Id));
        if (moneyMovement is null)
            return changesContainer;
        MoneyMovements.Remove(moneyMovement);
    
        await SaveChangesAsync();
    
        changesContainer.MoneyMovements.Add((moneyMovement, ChangeKind.Deleted));
        return changesContainer;
    }
    
    public async Task<ChangesContainer> DeleteCategoryAsync(CategoryId categoryId, UserId userId)
    {
        var changesContainer = CreateChangesContainer();
    
        var category =
            await Categories.FirstOrDefaultAsync(_ => _.Id.Equals(categoryId.Id) && _.UserId.Equals(userId.Id));
        if (category is null)
            return changesContainer;
    
        // delete all money movements that are assosicated with this category
        // todo: maybe this implicit behaviour should be separated as "domain logic". The repository shouldn't be responsible for that.
        var categoryMoneyMovements = MoneyMovements.Where(_ => _.CategoryId.Equals(categoryId.Id)).ToList();
    
        MoneyMovements.RemoveRange(categoryMoneyMovements);
        Categories.Remove(category);
        await SaveChangesAsync();
    
        changesContainer.Categories.Add((category, ChangeKind.Deleted));
        foreach (var moneyMovement in categoryMoneyMovements)
            changesContainer.MoneyMovements.Add((moneyMovement, ChangeKind.Deleted));
    
        return changesContainer;
    }
    
    public async Task AddTemplateAsync(Template template)
    {
        await Templates.AddAsync(template);
        await SaveChangesAsync();
    }
    
    public Task<List<Template>> GetTemplates(UserId userId)
    {
        return Task.FromResult(Templates.Where(_ => _.UserId.Equals(userId.Id)).ToList());
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