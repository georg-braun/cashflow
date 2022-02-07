using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace budet_backend_integration_tests;

public class IntegrationTest
{
    public IntegrationTest()
    {
        var appFactory = new WebApplicationFactory<Program>();
        client = appFactory.CreateClient();
    }

    protected HttpClient client { get; set; }
}