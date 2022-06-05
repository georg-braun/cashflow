using System.Linq;
using budet_backend_integration_tests;
using budget_backend.data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace budget_backend_integration_tests.backend;

/// <summary>
///     Creates a backend with authentication and a sqlite database.
/// </summary>
/// <typeparam name="TStartup"></typeparam>
public class AuthenticatedSqLiteWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    private readonly SqliteConnection _connection;

    public AuthenticatedSqLiteWebApplicationFactory(SqliteConnection connection)
    {
        _connection = connection;
    }

    private void RemoveRegisteredDataContext(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType ==
                 typeof(DbContextOptions<CashflowDataContext>));

        if (descriptor is null) return;
        services.Remove(descriptor);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            RemoveRegisteredDataContext(services);
            AddSqliteDbContext(services);
            EnsureThatDatabaseIsCreated(services);
        });

        ConfigureTestJwtOptions(builder);
        //ConfigureAuth0Authentication(builder);
    }

    private void ConfigureAuth0Authentication(IWebHostBuilder builder)
    {
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<MoneyMovementTests>()
            .Build();

        builder.ConfigureAppConfiguration(_ => _.AddConfiguration(config));
    }

    private void ConfigureTestJwtOptions(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = FakeJwtManager.SecurityKey,
                    ValidIssuer = FakeJwtManager.Issuer,
                    ValidAudience = FakeJwtManager.Audience
                };
            });
        });
    }

    private void EnsureThatDatabaseIsCreated(IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();

        using var scope = sp.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<CashflowDataContext>();

        db.Database.EnsureCreated();
    }

    private void AddSqliteDbContext(IServiceCollection services)
    {
        services.AddDbContext<CashflowDataContext>(options => { options.UseSqlite(_connection); });
    }
}