namespace OnOff.Todo.Api.Application.DTOs
{
    // Esta clase representa cómo devolvemos una tarea al frontend
    public class TodoTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}


