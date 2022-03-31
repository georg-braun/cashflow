using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Data.Sqlite;

namespace budget_backend_integration_tests.backend;


/// <summary>
///     Create a backend and offer a client for interaction.
/// </summary>
public class IntegrationTestBackend : IDisposable
{
    private readonly SqliteConnection _connection = new("Filename=:memory:");

    public IntegrationTestBackend()
    {
        StartSqliteConnection();

        var appFactory = new AuthenticatedSqLiteWebApplicationFactory<Program>(_connection);
        client = appFactory.CreateClient();
        
        // add the bearer token
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", AuthenticationService.AccessToken);
    }

    public HttpClient client { get; set; }

    public void Dispose()
    {
        // Sqlite in-memory data is cleared with connection dispose.
        _connection.Dispose();
    }

    private void StartSqliteConnection()
    {
        // Open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose).
        _connection.Open();
    }
}