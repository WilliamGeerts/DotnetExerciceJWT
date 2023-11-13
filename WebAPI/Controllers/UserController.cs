using System.Security.Claims;
using ExerciceJWT.Dtos;
using ExerciceJWT.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExerciceJWT.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly TokenService _tokenService;

    public UserController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    //AUTH
    [HttpPost("auth")]
    public IActionResult Auth(DtoInputLogin loginDto)
    {
        // Vérifiez ici si le login et le mot de passe sont corrects (simulé pour l'exemple)
        if (true)
        {
            // Générez le token JWT
            var token = _tokenService.GenerateToken(loginDto);
            
            // Ajouter le token au claims du cookie
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, token) };
            
            //Faire du token l'identity du cookie
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Émettez le cookie d'authentification avec le token JWT
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return Ok();
        }
        return Unauthorized(new { message = "Échec de l'authentification. Veuillez vérifier vos identifiants." });
    }

    //ISCONNECTED
    [HttpGet("isConnected")]
    [Authorize]
    public IActionResult IsConnected() { return Ok(); }
}