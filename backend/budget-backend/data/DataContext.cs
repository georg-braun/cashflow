using budget_backend.data.dbDto;
using Microsoft.EntityFrameworkCore;

namespace budget_backend.data;


public class DataContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DataContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(
            _configuration["ConnectionStrings:Database"]);
    public DbSet<Account> Accounts { get; set; }
    
}