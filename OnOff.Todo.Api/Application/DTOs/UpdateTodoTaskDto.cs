using System.ComponentModel.DataAnnotations;

namespace OnOff.Todo.Api.Application.DTOs
{
    // Esta clase representa los datos que el frontend envía
    // cuando quiere modificar una tarea existente
    public class UpdateTodoTaskDto
    {
        // Título de la tarea, obligatorio al actualizar
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        // Descripción opcional, puede ser cambiada o dejada en blanco
        [MaxLength(1000)]
        public string? Description { get; set; }

        // Estado de la tarea, true si está completada
        public bool IsCompleted { get; set; }
    }
}
