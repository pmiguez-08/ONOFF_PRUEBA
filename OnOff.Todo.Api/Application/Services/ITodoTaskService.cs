using OnOff.Todo.Api.Application.DTOs;

namespace OnOff.Todo.Api.Application.Services
{
    // Esta interfaz define las operaciones que nuestro servicio de tareas debe ofrecer
    // La usamos para poder cambiar la implementación sin afectar al resto del código
    public interface ITodoTaskService
    {
        // Obtiene la lista de tareas de un usuario, con un filtro por estado opcional
        Task<List<TodoTaskDto>> GetTasksAsync(int userId, string? statusFilter);

        // Crea una nueva tarea para un usuario
        Task<TodoTaskDto> CreateTaskAsync(int userId, CreateTodoTaskDto dto);

        // Actualiza una tarea existente
        Task<TodoTaskDto?> UpdateTaskAsync(int userId, int taskId, UpdateTodoTaskDto dto);

        // Elimina una tarea
        Task<bool> DeleteTaskAsync(int userId, int taskId);

        // Devuelve las métricas del dashboard: total, completadas y pendientes
        Task<(int total, int completed, int pending)> GetDashboardAsync(int userId);
    }
}
