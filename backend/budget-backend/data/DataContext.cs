using AutoMapper;
using budget_backend.data.dbDto;
using Microsoft.EntityFrameworkCore;

namespace budget_backend.data;

public class DataContext : DbContext
{
    private readonly IMapper _mapper;

    public DataContext(DbContextOptions options, IMapper mapper) : base(options)
    {
        _mapper = mapper;
    }

    private DbSet<Account> Accounts { get; set; }
    
    private DbSet<Transaction> Transactions { get; set; }

    public async Task AddAccountAsync(domain.Account account)
    {
        var dtoAccount = _mapper.Map<data.dbDto.Account>(account);
        
        var transactions = account.GetTransactions();
        var dtoTransactions = _mapper.Map<IEnumerable<data.dbDto.Transaction>>(transactions);

        await Accounts.AddAsync(dtoAccount);
        await Transactions.AddRangeAsync(dtoTransactions);
        await SaveChangesAsync();
    }
}