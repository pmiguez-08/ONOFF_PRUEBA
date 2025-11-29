using Microsoft.AspNetCore.Mvc;
using OnOff.Todo.Api.Application.DTOs;
using OnOff.Todo.Api.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace OnOff.Todo.Api.Controllers
{
    // Este atributo indica que este controlador responde a peticiones de API
    [ApiController]
    // Esta ruta define cómo se construye la URL de los endpoints de este controlador
    // api/todotasks será la base de la ruta
    [Route("api/[controller]")]
    public class TodoTasksController : ControllerBase
    {
        // Campo privado para guardar la referencia al servicio de tareas
        private readonly ITodoTaskService _todoTaskService;

        // El constructor recibe el servicio por inyección de dependencias
        public TodoTasksController(ITodoTaskService todoTaskService)
        {
            // Guardamos el servicio en el campo para usarlo en los métodos del controlador
            _todoTaskService = todoTaskService;
        }

        // Este método obtiene la lista de tareas
        // Responde a una petición HTTP GET en la ruta api/todotasks
        // El parámetro status es opcional y se usa para filtrar por estado
        [HttpGet]
        public async Task<ActionResult<List<TodoTaskDto>>> GetTasks([FromQuery] string? status)
        {
            // Más adelante el userId vendrá del token JWT
            // Por ahora usamos un valor fijo para simplificar el ejemplo
            var userId = 1;

            // Llamamos al servicio para obtener las tareas del usuario con el filtro de estado
            var tasks = await _todoTaskService.GetTasksAsync(userId, status);

            // Devolvemos las tareas con un código de estado 200 OK
            return Ok(tasks);
        }

        // Este método crea una nueva tarea
        // Responde a una petición HTTP POST en api/todotasks
        // El cuerpo de la petición debe contener los datos de CreateTodoTaskDto
        [HttpPost]
        public async Task<ActionResult<TodoTaskDto>> CreateTask([FromBody] CreateTodoTaskDto dto)
        {
            // Validamos el modelo usando las anotaciones de datos de la clase DTO
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, devolvemos un código 400 con los errores
                return BadRequest(ModelState);
            }

            // De nuevo, más adelante este userId vendrá del token
            var userId = 1;

            // Llamamos al servicio para crear la tarea
            var created = await _todoTaskService.CreateTaskAsync(userId, dto);

            // Devolvemos un código 201 Created con la tarea creada
            // y una cabecera Location que apunta al recurso recién creado
            return CreatedAtAction(nameof(GetTasks), new { id = created.Id }, created);
        }

        // Este método actualiza una tarea existente
        // Responde a una petición HTTP PUT en api/todotasks/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<TodoTaskDto>> UpdateTask(int id, [FromBody] UpdateTodoTaskDto dto)
        {
            // Validamos la información que llega desde el frontend
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolvemos 400 BadRequest
                return BadRequest(ModelState);
            }

            // De nuevo, el userId será tomado del token en la versión con JWT
            var userId = 1;

            // Llamamos al servicio para actualizar la tarea
            var updated = await _todoTaskService.UpdateTaskAsync(userId, id, dto);

            // Si el resultado es null, significa que la tarea no existe o no pertenece al usuario
            if (updated == null)
            {
                // Devolvemos un código 404 NotFound
                return NotFound();
            }

            // Si todo salió bien, devolvemos la tarea actualizada con código 200 OK
            return Ok(updated);
        }

        // Este método elimina una tarea
        // Responde a una petición HTTP DELETE en api/todotasks/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            // De nuevo, el userId será dinámico cuando tengamos JWT
            var userId = 1;

            // Llamamos al servicio para intentar eliminar la tarea
            var deleted = await _todoTaskService.DeleteTaskAsync(userId, id);

            // Si no se pudo eliminar, devolvemos 404 NotFound
            if (!deleted)
            {
                return NotFound();
            }

            // Si se eliminó correctamente, devolvemos 204 NoContent
            return NoContent();
        }

        // Este método devuelve las métricas del dashboard
        // Responde a una petición HTTP GET en api/todotasks/dashboard
        [HttpGet("dashboard")]
        public async Task<ActionResult<object>> GetDashboard()
        {
            // Más adelante el userId se obtendrá del token
            var userId = 1;

            // Llamamos al servicio para obtener las métricas del usuario
            var (total, completed, pending) = await _todoTaskService.GetDashboardAsync(userId);

            // Creamos un objeto anónimo para devolver los datos de manera clara
            var result = new
            {
                totalTasks = total,
                completedTasks = completed,
                pendingTasks = pending
            };

            // Devolvemos el objeto con código 200 OK
            return Ok(result);
        }
    }
}
