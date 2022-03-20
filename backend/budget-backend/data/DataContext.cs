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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Spending composite primary key
        modelBuilder.Entity<SpendingDto>().HasKey(item => new {item.AccountEntryId, item.BudgetaryItemId});
    }

    private DbSet<AccountDto> Accounts { get; set; } = null!;
    private DbSet<AccountEntryDto> AccountEntries { get; set; } = null!;

    private DbSet<AccountTransactionDto> AccountTransactions { get; set; } = null!;
    private DbSet<BudgetaryItemDto> BudgetaryItems { get; set; } = null!;
    private DbSet<BudgetEntryDto> BudgetEntries { get; set; } = null!;
    private DbSet<SpendingDto> Spendings { get; set; } = null!;


    public async Task AddAccountAsync(Account account)
    {
        var accountDto = account.ToDbDto();
        await Accounts.AddAsync(accountDto);
        await SaveChangesAsync();
    }

    public Account? GetAccount(string accountName)
    {
        var accountDto = Accounts.FirstOrDefault(_ => _.Name.Equals(accountName));
        return accountDto?.ToDomain();
    }

    public async Task AddAccountEntryAsync(AccountEntry accountEntry)
    {
        var accountEntryDto = accountEntry.ToDbDto();
        await AccountEntries.AddAsync(accountEntryDto);
        await SaveChangesAsync();
    }

    public async Task AddSpendingAsync(AccountEntry accountEntry, Spending spending)
    {
        var accountEntryDto = accountEntry.ToDbDto();
        
        await AccountEntries.AddAsync(accountEntryDto);
        var spendingDto = spending.ToDbDto();
        await Spendings.AddAsync(spendingDto);
        await SaveChangesAsync();
    }

    public async Task AddBudgetaryItemAsync(BudgetaryItem budgetaryItem)
    {
        var budgetaryItemDbDto = budgetaryItem.ToDbDto();
        await BudgetaryItems.AddAsync(budgetaryItemDbDto);
        await SaveChangesAsync();
    }

    public IEnumerable<BudgetaryItem> GetBudgetaryItems() => BudgetaryItems.Select(_ => _.ToDomain());

    public async Task AddBudgetEntryAsync(BudgetEntry budgetEntry)
    {
        await BudgetEntries.AddAsync(budgetEntry.ToDbDto());
        await SaveChangesAsync();
    }

    public IEnumerable<BudgetEntry> GetBudgetEntries(BudgetaryItemId budgetaryItemId) => BudgetEntries
        .Where(_ => _.BudgetaryItemId.Equals(budgetaryItemId.Id)).Select(_ => _.ToDomain());
    

    public IEnumerable<Account> GetAccounts()
    {
        return Accounts.Select(_ => _.ToDomain());
    }

    public IEnumerable<AccountEntry> GetAccountEntries(AccountId accountId)
    {
        var accountEntryDtos = AccountEntries.Where(_ => _.AccountId.Equals(accountId.Id));
        return accountEntryDtos.Select(_ => _.ToDomain());
    }

    public IEnumerable<Spending> GetSpendings() => Spendings.Select(_ => _.ToDomain());

    public async Task<AccountEntry?> GetAccountEntryAsync(AccountEntryId accountEntryId)
    {
        var accountEntryDto = await AccountEntries.SingleOrDefaultAsync(_ => _.Id.Equals(accountEntryId.Id));
        return accountEntryDto?.ToDomain();
    }
}