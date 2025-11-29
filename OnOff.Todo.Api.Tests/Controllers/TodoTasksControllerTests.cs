using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnOff.Todo.Api.Application.DTOs;
using OnOff.Todo.Api.Application.Services;
using OnOff.Todo.Api.Controllers;
using Xunit;

namespace OnOff.Todo.Api.Tests.Controllers
{
    // Esta clase contiene pruebas unitarias para el controlador de tareas
    public class TodoTasksControllerTests
    {
      [Fact]
      public async Task GetTasks_Should_Return_Ok_With_Task_List()
      {
          // Creamos una lista de tareas de ejemplo
          var tasks = new List<TodoTaskDto>
          {
              new TodoTaskDto { Id = 1, Title = "Tarea 1", IsCompleted = false },
              new TodoTaskDto { Id = 2, Title = "Tarea 2", IsCompleted = true }
          };

          // Creamos un mock del servicio de tareas
          var serviceMock = new Mock<ITodoTaskService>();

          // Configuramos el mock para que cuando se llame a GetTasksAsync devuelva la lista de ejemplo
          serviceMock
              .Setup(s => s.GetTasksAsync(It.IsAny<int>(), It.IsAny<string?>()))
              .ReturnsAsync(tasks);

          // Creamos una instancia del controlador usando el mock del servicio
          var controller = new TodoTasksController(serviceMock.Object);

          // Ejecutamos el método del controlador
          var result = await controller.GetTasks(null);

          // Verificamos que el resultado sea un OkObjectResult
          var okResult = Assert.IsType<OkObjectResult>(result.Result);

          // Verificamos que el contenido del Ok sea la lista de tareas
          var returnedTasks = Assert.IsAssignableFrom<List<TodoTaskDto>>(okResult.Value);

          // Verificamos que la lista tenga 2 elementos
          Assert.Equal(2, returnedTasks.Count);
      }
    }
}
