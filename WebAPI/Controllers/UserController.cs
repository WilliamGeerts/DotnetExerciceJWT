using ExerciceJWT.Dtos;
using ExerciceJWT.Services;
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

            // Émettez le cookie avec le token JWT
            Response.Cookies.Append("CookieJWT", token, new CookieOptions
            {
                /*
                HttpOnly = true : Cette option indique que le cookie ne sera accessible que par le serveur via HTTP, 
                et pas via JavaScript dans le navigateur. Cela renforce la sécurité en empêchant les attaques XSS 
                (Cross-Site Scripting) qui pourraient essayer d'accéder au cookie depuis le côté client.
                
                Secure = true : Cette option indique que le cookie ne sera transmis que via une connexion HTTPS 
                sécurisée. Cela garantit que le cookie ne sera pas envoyé via une connexion HTTP non sécurisée, 
                ce qui renforce la sécurité de la transmission du token JWT.
                
                SameSite = SameSiteMode.None : Cette option détermine la politique SameSite 
                du cookie. Dans cet exemple, SameSiteMode.None est utilisé, ce qui signifie que le cookie sera envoyé 
                même pour les requêtes provenant d'autres sites (cross-site). C'est souvent nécessaire pour 
                que l'authentification fonctionne correctement avec des applications front-end distinctes 
                (comme une application Angular) qui envoient des requêtes vers l'API du serveur.
                */
                
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok();
        }

        return Unauthorized(new { message = "Échec de l'authentification. Veuillez vérifier vos identifiants." });
    }

    //ISCONNECTED
    [HttpGet("isConnected")]
    [Authorize]
    public IActionResult IsConnected() { return Ok(); }
}