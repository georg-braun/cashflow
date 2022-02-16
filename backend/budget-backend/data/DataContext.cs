using AutoMapper;
using budget_backend.data.dbDto;
using budget_backend.domain;
using Microsoft.EntityFrameworkCore;

namespace budget_backend.data;

public class DataContext : DbContext
{
    private readonly IMapper _mapper;

    public DataContext(DbContextOptions options, IMapper mapper) : base(options)
    {
        _mapper = mapper;
    }

    private DbSet<AccountDto> Accounts { get; set; }
    
    private DbSet<TransactionDto> Transactions { get; set; }

    public async Task AddAccountAsync(domain.Account account)
    {
        var dtoAccount = _mapper.Map<data.dbDto.AccountDto>(account);
        
        var transactions = account.GetTransactions();
        var dtoTransactions = _mapper.Map<IEnumerable<data.dbDto.TransactionDto>>(transactions);

        await Accounts.AddAsync(dtoAccount);
        await Transactions.AddRangeAsync(dtoTransactions);
        await SaveChangesAsync();
    }

    public Task<bool> GetAccountAsync(string accountName, out Account? account)
    {
        account = null;
        var dtoAccount = Accounts.FirstOrDefault(_ => _.Name.Equals(accountName));
        if (dtoAccount is null)
        {
            return Task.FromResult(true);
        }
        
        account = _mapper.Map<AccountDto, Account>(dtoAccount);
        return Task.FromResult(account is not null);
    }
}