using StudentManagement.DTO;
using StudentManagement.Model;

namespace StudentManagement.Services.IServices
{
    public interface ILoginService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto dto);

    }
}
