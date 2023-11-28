using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DomainHelpers.Domain.Authentication;
public class PublicKeyJwtTokenHandler {
    public const string Algorithm = SecurityAlgorithms.HmacSha256;

    public string Generate(
        string key,
        string issuer,
        string audience,
        string kid,
        ClaimsIdentity? subject = null,
        DateTime? expires = null
    ) {
        var handler = new JwtSecurityTokenHandler();
        var k = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(k, Algorithm);

        try {
            var token = handler.CreateJwtSecurityToken(issuer, audience, subject, null, expires, null, credentials);
            return handler.WriteToken(token);
        }
        catch {
            throw;
        }
    }

    public ClaimsPrincipal? Validate(string token, string key, string issuer, string audience) {
        var handler = new JwtSecurityTokenHandler();

        try {
            return handler.ValidateToken(
                token,
                new() {
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                },
                out _);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return null;
        }
    }
}