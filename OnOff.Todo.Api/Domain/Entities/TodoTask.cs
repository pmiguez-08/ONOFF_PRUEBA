using System;

namespace OnOff.Todo.Api.Domain.Entities
{
    // Esta clase representa una tarea en la lista To Do del usuario
    public class TodoTask
    {
        // Identificador único de la tarea
        public int Id { get; set; }

        // Título corto de la tarea
        public string Title { get; set; } = string.Empty;

        // Descripción detallada opcional
        public string? Description { get; set; }

        // Indica si la tarea está completada
        public bool IsCompleted { get; set; }

        // Fecha de creación de la tarea
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Fecha de última actualización de la tarea
        public DateTime? UpdatedAt { get; set; }

        // Id del usuario propietario de la tarea
        public int UserId { get; set; }

        // Referencia al usuario propietario de la tarea
        public ApplicationUser? User { get; set; }
    }
}

