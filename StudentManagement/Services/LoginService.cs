using StudentManagement.DTO;
using StudentManagement.Model;
using StudentManagement.Respository.IRepository;
using StudentManagement.Services.IServices;
using System.Threading.Tasks;

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
            // Fetch student by email
            var student = await _studentRepository.GetByEmailAsync(dto.Email);

            // Compare password directly (Plain Text Check)
            if (student != null && student.Password == dto.Password)
            {
                return student;
            }

            return null;
        }
    }
}
