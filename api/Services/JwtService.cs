using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FullStackPoc.Api.Models;
using Microsoft.IdentityModel.Tokens;

namespace FullStackPoc.Api.Services;
public sealed class JwtService(IConfiguration config)
{
    public string CreateToken(User user)
    {
        var claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, user.Id ?? ""), new Claim(ClaimTypes.Name, user.Name), new Claim(ClaimTypes.Email, user.Email) };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(double.Parse(config["Jwt:ExpiryMinutes"] ?? "60")), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
