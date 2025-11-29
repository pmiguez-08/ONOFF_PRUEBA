using System.ComponentModel.DataAnnotations;

namespace OnOff.Todo.Api.Application.DTOs
{
    // Esta clase representa los datos que el usuario envía cuando quiere iniciar sesión
    public class LoginRequestDto
    {
        // El correo electrónico del usuario, es obligatorio
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // La contraseña que el usuario escribe, también es obligatoria
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
