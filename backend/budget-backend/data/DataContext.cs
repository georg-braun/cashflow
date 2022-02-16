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
    
    private DbSet<TransactionDto> Transactions { get; set; }

    public async Task AddAccountAsync(Account account)
    {
        var dtoAccount = account.ToDbDto(); 
        
        var transactions = account.GetTransactions();
        var dtoTransactions = transactions.Select(_ => _.ToDbDto());

        await Accounts.AddAsync(dtoAccount);
        await Transactions.AddRangeAsync(dtoTransactions);
        await SaveChangesAsync();
    }

    public Task<bool> GetAccountAsync(string accountName, out Account? account)
    {
        account = null;
        var accountDto = Accounts.FirstOrDefault(_ => _.Name.Equals(accountName));
        if (accountDto is null)
        {
            return Task.FromResult(true);
        }

        var transactionDtos = Transactions.Where(_ => _.AccountId.Equals(accountDto.Id));

        account = accountDto.ToDomain(transactionDtos);
        return Task.FromResult(account is not null);
    }
}