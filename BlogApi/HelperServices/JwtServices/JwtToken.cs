using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogApi.Entities;
using BlogApi.HelperEntities;
using BlogApi.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.HelperServices.JwtServices;


public class JwtToken : IJwtToken
{
    private readonly JwtOptions _jwtOptions;

    public JwtToken(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string CreateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email)
        };

        var secretKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(_jwtOptions.SecretKey));

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var utcNow = DateTime.UtcNow;

        var jwtSecurityToken = new JwtSecurityToken
        (
            issuer: _jwtOptions.ValidIssuer,
            audience: _jwtOptions.ValidAudience,
            claims: claims,
            signingCredentials: signingCredentials,
            notBefore: utcNow,
            expires: utcNow.AddMinutes(_jwtOptions.Expires)
        );

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return jwtToken;
    }

}