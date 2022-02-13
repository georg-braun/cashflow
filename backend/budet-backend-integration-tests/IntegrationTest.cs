using System;
using System.Linq;
using System.Net.Http;
using budget_backend.data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace budet_backend_integration_tests;

public class IntegrationTest
{
    private readonly DbContextOptions<DataContext> _contextOptions;
    private readonly SqliteConnection _connection;

    public IntegrationTest()
    {
        
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:"); 
        _connection.Open();
        
        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite(_connection)
            .Options;

        // Create the schema and seed some data
        using var context = new DataContext(_contextOptions);

        context.Database.EnsureCreated();
        
        // ADD + SAVE

        var appFactory = new CustomWebApplicationFactory<Program>();
        
        client = appFactory.CreateClient();
    }

    DataContext CreateContext() => new DataContext(_contextOptions);
    
    public void Dispose() => _connection.Dispose();

    protected HttpClient client { get; set; }
}

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup: class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<DataContext>));

            services.Remove(descriptor);

            services.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<DataContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                db.Database.EnsureCreated();
                
            }
        });
    }
}