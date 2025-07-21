using StudentManagement.DTO;
using StudentManagement.Model;
using StudentManagement.Respository.IRepository;
using StudentManagement.Services.IServices;

namespace StudentManagement.Services
{
    public class LoginService : ILoginService
    {
        private readonly IStudentRepository _studentRepository;

        public LoginService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student> LoginAsync(LoginDto dto)
        {
            var student = await _studentRepository.GetByEmailAsync(dto.Email);
            if (student != null && student.Password == dto.Password)
            {
                return student;
            }
            return null;
        }
    }
}
