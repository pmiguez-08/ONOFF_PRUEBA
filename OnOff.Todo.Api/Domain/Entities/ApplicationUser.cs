using Microsoft.AspNetCore.Identity;

namespace OnOff.Todo.Api.Domain.Entities
{
    // Esta clase representa al usuario de la aplicación usando ASP.NET Core Identity
    public class ApplicationUser : IdentityUser<int>
    {
        // Nombre completo del usuario para mostrar en la interfaz
        public string FullName { get; set; } = string.Empty;
    }
}

