using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LeadTrackApi.Application.Services;

public class JwtService
{
    private readonly string _secretKey;
    private readonly string _iusser;
    private readonly string _audience;
    private const int TokenExpirationMinutes = 60;

    public JwtService(IConfiguration configuration)
    {
        _secretKey = configuration["Security:SecretKey"] ?? throw new ArgumentNullException(nameof(configuration), "SecretKey cannot be null");
        _iusser = configuration["Security:Issuer"] ?? throw new ArgumentNullException(nameof(configuration), "Issuer cannot be null");
        _audience = configuration["Security:Audience"] ?? throw new ArgumentNullException(nameof(configuration), "Audience cannot be null");
    }

    public string GenerateToken(string email, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: _iusser,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(TokenExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
