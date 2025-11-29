using OnOff.Todo.Api.Application.DTOs;

namespace OnOff.Todo.Api.Application.Services
{
    public interface IUserService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    }
}
