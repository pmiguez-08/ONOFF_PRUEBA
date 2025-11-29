using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnOff.Todo.Api.Application.DTOs;
using OnOff.Todo.Api.Application.Services;
using OnOff.Todo.Api.Domain.Entities;
using OnOff.Todo.Api.Infrastructure.Data;
using Xunit;

namespace OnOff.Todo.Api.Tests.Services
{
    // Esta clase contiene pruebas unitarias para el servicio de tareas
    public class TodoTaskServiceTests
    {
        // Este método crea un ApplicationDbContext en memoria para usarlo en las pruebas
        private ApplicationDbContext CreateInMemoryContext()
        {
            // Creamos opciones para que el contexto use una base de datos en memoria
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Devolvemos una nueva instancia del contexto usando esas opciones
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetTasksAsync_Should_Return_Only_Completed_Tasks_For_User()
        {
            // Aquí preparamos el escenario de la prueba

            // Creamos un contexto en memoria
            using var context = CreateInMemoryContext();

            // Creamos un usuario de ejemplo
            var user = new ApplicationUser
            {
                Id = 1,
                UserName = "demo@onoff.com",
                NormalizedUserName = "DEMO@ONOFF.COM",
                Email = "demo@onoff.com",
                NormalizedEmail = "DEMO@ONOFF.COM",
                FullName = "Usuario Demo",
                EmailConfirmed = true
            };

            // Agregamos el usuario al contexto
            context.Users.Add(user);

            // Creamos dos tareas: una completada y otra pendiente
            var taskCompleted = new TodoTask
            {
                Id = 1,
                Title = "Tarea completada",
                IsCompleted = true,
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id
            };

            var taskPending = new TodoTask
            {
                Id = 2,
                Title = "Tarea pendiente",
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id
            };

            // Agregamos las tareas al contexto
            context.TodoTasks.AddRange(taskCompleted, taskPending);

            // Guardamos los cambios en la base de datos en memoria
            await context.SaveChangesAsync();

            // Creamos una instancia del servicio usando el contexto
            var service = new TodoTaskService(context);

            // Aquí ejecutamos la acción que queremos probar

            // Llamamos al método para obtener solo las tareas completadas
            var result = await service.GetTasksAsync(user.Id, "completed");

            // Aquí verificamos que el resultado sea el esperado

            // Debe devolver exactamente 1 tarea
            Assert.Single(result);

            // La única tarea devuelta debe ser la que está completada
            Assert.Equal("Tarea completada", result[0].Title);
            Assert.True(result[0].IsCompleted);
        }

        [Fact]
        public async Task CreateTaskAsync_Should_Create_Task_For_User()
        {
            // Preparamos el contexto en memoria
            using var context = CreateInMemoryContext();

            // Creamos un usuario de ejemplo
            var user = new ApplicationUser
            {
                Id = 1,
                UserName = "demo@onoff.com",
                NormalizedUserName = "DEMO@ONOFF.COM",
                Email = "demo@onoff.com",
                NormalizedEmail = "DEMO@ONOFF.COM",
                FullName = "Usuario Demo",
                EmailConfirmed = true
            };

            // Agregamos el usuario al contexto
            context.Users.Add(user);

            await context.SaveChangesAsync();

            // Creamos el servicio de tareas usando el contexto
            var service = new TodoTaskService(context);

            // Creamos un DTO con los datos de la nueva tarea
            var createDto = new CreateTodoTaskDto
            {
                Title = "Nueva tarea de prueba",
                Description = "Descripción de la tarea de prueba"
            };

            // Ejecutamos el método que queremos probar
            var result = await service.CreateTaskAsync(user.Id, createDto);

            // Verificamos que la tarea devuelta tenga un Id mayor que 0
            Assert.True(result.Id > 0);

            // Verificamos que el título coincida con lo que enviamos
            Assert.Equal("Nueva tarea de prueba", result.Title);

            // Verificamos que la tarea esté marcada como no completada
            Assert.False(result.IsCompleted);

            // También verificamos que realmente se guardó en la base de datos
            var savedTask = await context.TodoTasks.FirstOrDefaultAsync(t => t.Id == result.Id);

            Assert.NotNull(savedTask);
            Assert.Equal(user.Id, savedTask!.UserId);
        }
    }
}
