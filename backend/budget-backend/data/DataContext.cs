using System.Linq;
using budget_backend.application;
using budget_backend.data.dbDto;
using budget_backend.domain;
using budget_backend.domain.account;
using budget_backend.domain.budget;
using Microsoft.EntityFrameworkCore;

namespace budget_backend.data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }
    
    private DbSet<UserDto> Users { get; set; } = null!;
    private DbSet<AccountDto> Accounts { get; set; } = null!;
    private DbSet<AccountEntryDto> AccountEntries { get; set; } = null!;

    private DbSet<AccountTransactionDto> AccountTransactions { get; set; } = null!;
    private DbSet<BudgetaryItemDto> BudgetaryItems { get; set; } = null!;
    private DbSet<BudgetEntryDto> BudgetEntries { get; set; } = null!;

    public async Task AddAccountAsync(Account account, UserId userId)
    {
        var accountDto = account.ToDbDto(userId);
        await Accounts.AddAsync(accountDto);
        await SaveChangesAsync();
    }

    public async Task AddAccountEntryAsync(AccountEntry accountEntry, UserId userId)
    {
        var accountEntryDto = accountEntry.ToDbDto(userId);
        await AccountEntries.AddAsync(accountEntryDto);
        await SaveChangesAsync();
    }

    public async Task AddBudgetaryItemAsync(BudgetaryItem budgetaryItem, UserId userId)
    {
        var budgetaryItemDbDto = budgetaryItem.ToDbDto(userId);
        await BudgetaryItems.AddAsync(budgetaryItemDbDto);
        await SaveChangesAsync();
    }

    public IEnumerable<BudgetaryItem> GetBudgetaryItems(UserId userId) => BudgetaryItems.Where(_ => _.UserId.Equals(userId.Id)).Select(_ => _.ToDomain());

    public async Task AddBudgetEntryAsync(BudgetEntry budgetEntry, UserId userId)
    {
        await BudgetEntries.AddAsync(budgetEntry.ToDbDto(userId));
        await SaveChangesAsync();
    }

    public IEnumerable<BudgetEntry> GetBudgetEntries(BudgetaryItemId budgetaryItemId, UserId userId) => BudgetEntries
        .Where(_ => _.UserId.Equals(userId.Id) && _.BudgetaryItemId.Equals(budgetaryItemId.Id)).Select(_ => _.ToDomain());
    

    public IEnumerable<Account> GetAccounts(UserId userId)
    {
        return Accounts.Where(_ => _.UserId.Equals(userId.Id)).Select(_ => _.ToDomain());
    }

    public IEnumerable<AccountEntry> GetAccountEntries(AccountId accountId, UserId userId)
    {
        var accountEntryDtos = AccountEntries.Where(_ => _.UserId.Equals(userId.Id) && _.AccountId.Equals(accountId.Id));
        return accountEntryDtos.Select(_ => _.ToDomain());
    }

    public async Task<AccountEntry?> GetAccountEntryAsync(AccountEntryId accountEntryId, UserId userId)
    {
        var accountEntryDto = await AccountEntries.SingleOrDefaultAsync(_ => _.Id.Equals(accountEntryId.Id));
        if (accountEntryDto != null && accountEntryDto.UserId.Equals(userId.Id))
            return accountEntryDto.ToDomain();
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
        
        var addedUserEntity = await Users.AddAsync(new UserDto(Guid.NewGuid(), authProviderId, DateTime.UtcNow));
        return UserId.New(addedUserEntity.Entity.Id);
    }

    public async Task<bool> DeleteAccountAsync(Guid accountId, UserId userId)
    {
        var account = await Accounts.FirstOrDefaultAsync(_ => _.Id.Equals(accountId) && _.UserId.Equals(userId.Id));
        if (account is null)
            return false;
        var removeResult = Accounts.Remove(account);
        var hResult = removeResult.State == EntityState.Deleted; 
        await SaveChangesAsync();
        return hResult;
    }

    public async Task<bool> DeleteAccountEntryAsync(Guid accountEntryId, UserId userId)
    {
        var accountEntry = await AccountEntries.FirstOrDefaultAsync(_ => _.Id.Equals(accountEntryId) && _.UserId.Equals(userId.Id));
        if (accountEntry is null)
            return false;
        var removeResult = AccountEntries.Remove(accountEntry);
        var hResult = removeResult.State == EntityState.Deleted; 
        await SaveChangesAsync();
        return hResult;
    }

    public async Task<bool> DeleteBudgetaryItemAsync(Guid budgetaryItemId, UserId userId)
    {
        var budgetaryItem = await BudgetaryItems.FirstOrDefaultAsync(_ => _.Id.Equals(budgetaryItemId) && _.UserId.Equals(userId.Id));
        if (budgetaryItem is null)
            return false;
        var removeResult = BudgetaryItems.Remove(budgetaryItem);
        var hResult = removeResult.State == EntityState.Deleted; 
        await SaveChangesAsync();
        return hResult;
    }
}