using System;
using System.IO;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Microsoft.Extensions.Configuration;

namespace budet_backend_integration_tests.backend;

public class AuthorizationServer
{
    public static string AccessToken => _accessToken.Value;
    private static Lazy<string> _accessToken = new(GetAccessToken().Result);

  

    private static IConfigurationSection GetAuthSettings()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .AddUserSecrets<AccountApiTests>()
            .Build()
            .GetSection("Auth0Client");
    }

    private static async Task<string> GetAccessToken()
    {
        var authSettings = GetAuthSettings();
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