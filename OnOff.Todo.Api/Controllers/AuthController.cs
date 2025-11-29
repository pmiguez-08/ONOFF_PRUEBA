using Microsoft.AspNetCore.Mvc;
using OnOff.Todo.Api.Application.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OnOff.Todo.Api.Application.Services;

namespace OnOff.Todo.Api.Controllers
{
    // Indicamos que este es un controlador de API.
    [ApiController]
    // La ruta base será: api/Auth  (porque el nombre de la clase es AuthController)
    [Route("api/[controller]")]   // → api/auth
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        // POST: api/Auth/login
        // Este endpoint será llamado por Angular cuando el usuario hace login.
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            // Primero validamos que el modelo esté bien (por ejemplo que no vengan nulos).
            if (!ModelState.IsValid)
            {
                // Si algo está mal en la petición, devolvemos 400 BadRequest con detalles.
                return BadRequest(ModelState);
            }

            // Aquí validamos las credenciales.
            // Para la prueba usamos un usuario fijo.
            // En un proyecto real esto vendría de la base de datos.
            var validEmail = "demo@onoff.com";
            var validPassword = "123456";

            if (request.Email != validEmail || request.Password != validPassword)
            {
                // Si el correo o la contraseña no coinciden, devolvemos 401 Unauthorized.
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            // Si las credenciales son correctas, generamos un token JWT.
            var token = GenerateJwtToken(request.Email);

            // Construimos la respuesta que Angular está esperando.
            var response = new LoginResponseDto
            {
                Token = token,
                UserName = "Usuario Demo",
                Email = request.Email
            };

            // Devolvemos 200 OK con el token y los datos del usuario.
            return Ok(response);
        }

        // Este método privado se encarga de crear un JWT sencillo.
        // No estamos usando configuración externa para no complicar la prueba.
        private string GenerateJwtToken(string email)
        {
            // Clave secreta para firmar el token.
            // En un proyecto real esto debería estar en appsettings.json y NUNCA en código.
            var secretKey = "clave-super-secreta-onoff-1234567890";

            // Convertimos la clave en bytes.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Indicamos que usaremos HmacSha256 para firmar el token.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Creamos los claims (datos que guardamos dentro del token).
            var claims = new[]
            {
                // Sub: sujeto del token (normalmente el usuario).
                new Claim(JwtRegisteredClaimNames.Sub, email),
                // Jti: identificador único del token.
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Construimos el objeto JwtSecurityToken.
            var token = new JwtSecurityToken(
                issuer: "OnOff.Todo.Api",     // Quién emite el token.
                audience: "OnOff.Todo.Web",   // Quién debería usar este token (el front).
                claims: claims,               // Los datos que pusimos arriba.
                expires: DateTime.UtcNow.AddHours(1), // Tiempo de expiración (1 hora).
                signingCredentials: creds     // Cómo se firma el token.
            );

            // Convertimos el objeto token en un string para enviarlo al frontend.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
