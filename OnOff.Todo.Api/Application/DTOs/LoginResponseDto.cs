namespace OnOff.Todo.Api.Application.DTOs
{
    // Esta clase representa la respuesta que la API devuelve después de un login exitoso
    public class LoginResponseDto
    {
        // El token JWT que el frontend debe guardar y enviar en las siguientes peticiones
        public string Token { get; set; } = string.Empty;

        // El nombre del usuario, se usa para mostrar información en el frontend
        public string UserName { get; set; } = string.Empty;

        // El correo del usuario, también se puede mostrar en el frontend
        public string Email { get; set; } = string.Empty;
    }
}
