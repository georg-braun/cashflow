using System;
using System.IO;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using budet_backend_integration_tests;
using Microsoft.Extensions.Configuration;

namespace budget_backend_integration_tests.backend;

public static class AuthenticationService
{
    public static string AccessToken => _accessToken.Value;
    private static readonly Lazy<string> _accessToken = new(GetFakeAccessTokenAsync().Result);

    private static IConfigurationSection GetAuth0Settings()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .AddUserSecrets<MoneyMovementTests>()
            .Build()
            .GetSection("Auth0Client");
    }

    private static async Task<string> GetFakeAccessTokenAsync()
    {
        return await Task.FromResult(FakeJwtManager.GenerateJwtToken());
    }    
    
    private static async Task<string> GetAuth0AccessTokenAsync()
    {
        var authSettings = GetAuth0Settings();
        var auth0Client = new AuthenticationApiClient(authSettings["Domain"]);
        var tokenRequest = new ClientCredentialsTokenRequest()
        {
            ClientId = authSettings["ClientId"],
            ClientSecret = authSettings["ClientSecret"],
            Audience = authSettings["Audience"]
        };
        var tokenResponse = await auth0Client.GetTokenAsync(tokenRequest);
        
        return tokenResponse.AccessToken;
    }
}