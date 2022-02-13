using System;
using System.Net.Http;
using Microsoft.Data.Sqlite;

namespace budet_backend_integration_tests;

public class IntegrationTest : IDisposable
{
    private SqliteConnection _connection;

    public IntegrationTest()
    {
        StartSqliteConnection();

        var appFactory = new CustomWebApplicationFactory<Program>(_connection);
        client = appFactory.CreateClient();
    }

    protected HttpClient client { get; set; }

    public void Dispose()
    {
        // Sqlite in-memory data is cleared with connection dispose.
        _connection.Dispose();
    }

    private void StartSqliteConnection()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
    }
}