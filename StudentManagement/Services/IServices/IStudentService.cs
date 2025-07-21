using StudentManagement.DTO;
using StudentManagement.Model;

namespace StudentManagement.Services.IServices
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(int id);
        Task AddStudentAsync(StudentDto dto);
        Task UpdateStudentAsync(int id, StudentDto dto);
        Task DeleteStudentAsync(int id);
    }

}
