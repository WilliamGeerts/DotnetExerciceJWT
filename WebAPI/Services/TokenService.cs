using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExerciceJWT.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace ExerciceJWT.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(DtoInputLogin user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            //Ajouter le login de l'utilisateur dans notre token
            Subject = new ClaimsIdentity(new Claim[] { new(ClaimTypes.Name, user.Login) }),

            //Ajouter la date d'expiration
            Expires = DateTime.UtcNow.AddHours(24),

            //Ajouter la signature
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public bool CheckValidity(string tokenString)
    {
        if (string.IsNullOrEmpty(tokenString)) return true;

        var jwtToken = new JwtSecurityToken(tokenString);
        var isConnected = jwtToken.ValidTo > DateTime.UtcNow && jwtToken.ValidFrom < DateTime.UtcNow;

        return isConnected;
    }
}