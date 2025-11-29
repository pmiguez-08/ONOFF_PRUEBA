using Microsoft.EntityFrameworkCore;
using OnOff.Todo.Api.Application.DTOs;
using OnOff.Todo.Api.Domain.Entities;
using OnOff.Todo.Api.Infrastructure.Data;

namespace OnOff.Todo.Api.Application.Services
{
    // Esta clase implementa la lógica de negocio relacionada con las tareas
    // Se encarga de hablar con la base de datos a través del ApplicationDbContext
    public class TodoTaskService : ITodoTaskService
    {
        // Campo privado para guardar el contexto de base de datos
        private readonly ApplicationDbContext _context;

        // El constructor recibe el contexto por inyección de dependencias
        public TodoTaskService(ApplicationDbContext context)
        {
            // Guardamos el contexto en el campo privado para usarlo en los métodos
            _context = context;
        }

        public async Task<List<TodoTaskDto>> GetTasksAsync(int userId, string? statusFilter)
        {
            // Empezamos creando una consulta base que trae las tareas del usuario
            var query = _context.TodoTasks
                .Where(t => t.UserId == userId)
                .AsQueryable();

            // Si el filtro de estado no es nulo ni vacío, aplicamos el filtro
            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                // Convertimos el filtro a minúsculas para evitar problemas de mayúsculas
                var normalized = statusFilter.ToLowerInvariant();

                // Si el filtro es "completed", solo traemos las tareas completadas
                if (normalized == "completed")
                {
                    query = query.Where(t => t.IsCompleted);
                }
                // Si el filtro es "pending", solo traemos las tareas pendientes
                else if (normalized == "pending")
                {
                    query = query.Where(t => !t.IsCompleted);
                }
                // Si el filtro es "all" u otra cosa, dejamos la consulta sin cambios
            }

            // Ejecutamos la consulta en la base de datos y obtenemos la lista de tareas
            var tasks = await query
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            // Convertimos las entidades de base de datos a DTOs para devolverlas al frontend
            var result = tasks.Select(t => new TodoTaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList();

            return result;
        }

        public async Task<TodoTaskDto> CreateTaskAsync(int userId, CreateTodoTaskDto dto)
        {
            // Creamos una nueva entidad de tarea a partir de los datos enviados por el frontend
            var entity = new TodoTask
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };

            // Agregamos la nueva tarea al contexto para que EF Core la rastree
            _context.TodoTasks.Add(entity);

            // Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devolvemos un DTO basado en la entidad recién guardada
            return new TodoTaskDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                IsCompleted = entity.IsCompleted,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public async Task<TodoTaskDto?> UpdateTaskAsync(int userId, int taskId, UpdateTodoTaskDto dto)
        {
            // Buscamos la tarea por su Id y por el Id del usuario
            var entity = await _context.TodoTasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            // Si no existe, devolvemos null para que el controlador sepa que no se encontró
            if (entity == null)
            {
                return null;
            }

            // Actualizamos las propiedades de la tarea con los datos enviados
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.IsCompleted = dto.IsCompleted;
            entity.UpdatedAt = DateTime.UtcNow;

            // Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devolvemos la versión actualizada como DTO
            return new TodoTaskDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                IsCompleted = entity.IsCompleted,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public async Task<bool> DeleteTaskAsync(int userId, int taskId)
        {
            // Buscamos la tarea que queremos eliminar
            var entity = await _context.TodoTasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            // Si no existe, devolvemos false para indicar que no se pudo eliminar
            if (entity == null)
            {
                return false;
            }

            // Marcamos la entidad para eliminarla
            _context.TodoTasks.Remove(entity);

            // Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devolvemos true para indicar que la eliminación fue exitosa
            return true;
        }

        public async Task<(int total, int completed, int pending)> GetDashboardAsync(int userId)
        {
            // Obtenemos todas las tareas del usuario actual
            var tasks = await _context.TodoTasks
                .Where(t => t.UserId == userId)
                .ToListAsync();

            // Calculamos el total de tareas
            var total = tasks.Count;

            // Calculamos cuántas están completadas
            var completed = tasks.Count(t => t.IsCompleted);

            // Calculamos cuántas están pendientes
            var pending = total - completed;

            // Devolvemos las tres métricas como una tupla
            return (total, completed, pending);
        }
    }
}
