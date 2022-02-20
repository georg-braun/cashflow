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


    public async Task AddAccountAsync(Account account)
    {
        var accountDto = account.ToDbDto();
        await Accounts.AddAsync(accountDto);
        await SaveChangesAsync();
    }

    public async Task<Account> GetAccountAsync(string accountName)
    {
        var accountDto = Accounts.FirstOrDefault(_ => _.Name.Equals(accountName));
        if (accountDto is null)
        {
            return null;
        }
        
        var account = accountDto.ToDomain();
        return account;
    }

    public async Task AddAccountEntryAsync(AccountEntry accountEntry)
    {
        var accountEntryDto = accountEntry.ToDbDto();
        AccountEntries.Update(accountEntryDto);
        await SaveChangesAsync();
    }

    public async Task AddSpendingAsync(AccountEntry accountEntry, Spending spending)
    {
        throw new NotImplementedException();
    }

    public async Task AddBudgetaryItemAsync(BudgetaryItem budgetaryItem)
    {
        throw new NotImplementedException();
    }

    public async Task AddBudgetChangeAsync(BudgetChange budgetChange)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteBudgetChangeAsync(Guid budgetChangeId)
    {
        throw new NotImplementedException();
    }
}