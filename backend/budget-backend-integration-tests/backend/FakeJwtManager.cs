using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace budget_backend_integration_tests.backend;

public class FakeJwtManager
{
    public static string Issuer { get; } = Guid.NewGuid().ToString();
    public static string Audience { get; } = Guid.NewGuid().ToString();
    public static SecurityKey SecurityKey { get; }
    public static SigningCredentials SigningCredentials { get; }

    private static readonly JwtSecurityTokenHandler TokenHandler = new();
    
    static FakeJwtManager()
    {
        var key = new byte[32];
        RandomNumberGenerator.Create().GetBytes(key);
        SecurityKey = new SymmetricSecurityKey(key) { KeyId = Guid.NewGuid().ToString() };
        SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
    }

    public static string GenerateJwtToken()
    {
        var claim = new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString());
        return TokenHandler.WriteToken(new JwtSecurityToken(Issuer, Audience, new []{claim}, null, DateTime.UtcNow.AddMinutes(60), SigningCredentials));
    }
}