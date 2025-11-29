using System.ComponentModel.DataAnnotations;

namespace OnOff.Todo.Api.Application.DTOs
{
    // Esta clase representa los datos que el frontend debe enviar
    // cuando quiere crear una nueva tarea
    public class CreateTodoTaskDto
    {
        // Título de la tarea, es obligatorio
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        // Descripción opcional de la tarea
        [MaxLength(1000)]
        public string? Description { get; set; }
    }
}
