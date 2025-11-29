namespace OnOff.Todo.Api.Domain.Entities
{
    // Esta clase representa a un usuario del sistema
    // Un usuario es la persona que va a iniciar sesión y gestionar sus tareas
    public class User
    {
        // Esta propiedad es el identificador único del usuario en la base de datos
        // Es de tipo entero porque es fácil de manejar y suficiente para la prueba
        public int Id { get; set; }

        // Esta propiedad guarda el nombre del usuario
        // Se usa solo para mostrar algo más amigable que el correo
        public string Name { get; set; } = string.Empty;

        // Esta propiedad guarda el correo electrónico del usuario
        // El correo será el dato que usaremos para que la persona pueda iniciar sesión
        public string Email { get; set; } = string.Empty;

        // Esta propiedad guarda la clave en forma de texto cifrado o hash
        // En una aplicación real nunca guardamos la contraseña en texto plano
        public string PasswordHash { get; set; } = string.Empty;

        // Esta propiedad indica si el usuario está activo
        // Nos permite deshabilitar usuarios sin borrarlos de la base de datos
        public bool IsActive { get; set; } = true;
    }
}
