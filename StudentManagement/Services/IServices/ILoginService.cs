using StudentManagement.DTO;
using StudentManagement.Model;

namespace StudentManagement.Services.IServices
{
    public interface ILoginService
    {
        Task<Student> LoginAsync(LoginDto dto);
    }
}
