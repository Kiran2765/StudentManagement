using StudentManagement.DTO;
using StudentManagement.Model;
using StudentManagement.Respository.IRepository;
using StudentManagement.Services.IServices;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repo;

        public StudentService(IStudentRepository repo)
        {
            _repo = repo;
        }

        public async Task AddStudentAsync(StudentDto dto)
        {
            var student = new Student
            {
                Name = dto.Name,
                Age = dto.Age,
                Email = dto.Email,
                Password = dto.Password
            };

            await _repo.AddAsync(student);
        }

        public async Task DeleteStudentAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task UpdateStudentAsync(int id, StudentDto dto)
        {
            var student = await _repo.GetByIdAsync(id);
            if (student != null)
            {
                student.Name = dto.Name;
                student.Age = dto.Age;
                student.Email = dto.Email;
                student.Password = dto.Password;

                await _repo.UpdateAsync(student);
            }
        }

       
    }
}
