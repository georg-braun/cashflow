using budget_backend.data.dbDto;
using Microsoft.EntityFrameworkCore;

namespace budget_backend.data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
}