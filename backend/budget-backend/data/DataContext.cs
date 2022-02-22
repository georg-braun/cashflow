using budget_backend.data.dbDto;
using budget_backend.domain;
using budget_backend.domain.account;
using budget_backend.domain.budget;
using Microsoft.EntityFrameworkCore;
using Spending = budget_backend.domain.budget.Spending;

namespace budget_backend.data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    private DbSet<AccountDto> Accounts { get; set; }
    private DbSet<AccountEntryDto> AccountEntries { get; set; }
    
    private DbSet<AccountTransactionDto> AccountTransactions { get; set; }
    private DbSet<BudgetaryItemDto> BudgetaryItems { get; set; }
    
    private DbSet<BudgetChangeDto> BudgetChanges { get; set; }


    public async Task AddAccountAsync(Account account)
    {
        var accountDto = account.ToDbDto();
        await Accounts.AddAsync(accountDto);
        await SaveChangesAsync();
    }

    public Task<Account> GetAccount(string accountName)
    {
        var accountDto = Accounts.FirstOrDefault(_ => _.Name.Equals(accountName));
        if (accountDto is null)
        {
            return Task.FromResult<Account>(null);
        }
        
        var account = accountDto.ToDomain();
        return Task.FromResult(account);
    }

    public async Task AddAccountEntryAsync(AccountEntry accountEntry)
    {
        var accountEntryDto = accountEntry.ToDbDto();
        await AccountEntries.AddAsync(accountEntryDto);
        await SaveChangesAsync();
    }

    public async Task AddSpendingAsync(AccountEntry accountEntry, Spending spending)
    {
        throw new NotImplementedException();
    }

    public async Task AddBudgetaryItemAsync(BudgetaryItem budgetaryItem)
    {
        var budgetaryItemDbDto = budgetaryItem.ToDbDto();
        await BudgetaryItems.AddAsync(budgetaryItemDbDto);
        await SaveChangesAsync();
    }

    public IEnumerable<BudgetaryItem> GetBudgetaryItems() => BudgetaryItems.Select(_ => _.ToDomain());

    public async Task AddBudgetChangeAsync(BudgetChange budgetChange)
    {
        var budgetChangeDto = budgetChange.ToDbDto();
        await BudgetChanges.AddAsync(budgetChangeDto);
        await SaveChangesAsync();
    }

    public IEnumerable<BudgetChange> GetBudgetChanges(Guid budgetaryItemId) => BudgetChanges
        .Where(_ => _.BudgetaryItemId.Equals(budgetaryItemId)).Select(_ => _.ToDomain());

    public async Task DeleteBudgetChangeAsync(Guid budgetChangeId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Account> GetAccounts()
    {
        return Accounts.Select(_ => _.ToDomain());
    }

    public IEnumerable<AccountEntry> GetAccountEntries(Guid accountId)
    {
        var accountEntryDtos = AccountEntries.Where(_ => _.AccountId.Equals(accountId));
        return accountEntryDtos.Select(_ => _.ToDomain());
    }
}