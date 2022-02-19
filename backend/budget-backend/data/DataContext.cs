using budget_backend.data.dbDto;
using budget_backend.domain;
using Microsoft.EntityFrameworkCore;

namespace budget_backend.data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    private DbSet<AccountDto> Accounts { get; set; }
    private DbSet<AccountEntryDto> AccountEntries { get; set; }
    
    //private DbSet<AccountTransactionDto> AccountTransactions { get; set; }


    public async Task AddAccountAsync(Account account)
    {
        var accountDto = account.ToDbDto();
        await Accounts.AddAsync(accountDto);

        var accountEntries = account.GetEntries().Select(_ => _.ToDbDto());
        await AccountEntries.AddRangeAsync(accountEntries);
        
        await SaveChangesAsync();
    }

    public async Task<Account> GetAccountAsync(string accountName)
    {
        //var accountDto = await Accounts.FindAsync(new[] {id});
   
        var accountDto = Accounts.FirstOrDefault(_ => _.Name.Equals(accountName));
        if (accountDto is null)
        {
            return null;
        }
        
        var accountEntryDtos = AccountEntries.Where(_ => _.AccountId.Equals(accountDto.Id));

        var account = accountDto.ToDomain();
        var accountEntries = accountEntryDtos.Select(_ => _.ToDomain(account));
        account.AddEntries(accountEntries);

        return account;
    }
}