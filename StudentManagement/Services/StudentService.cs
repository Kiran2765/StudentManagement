using StudentManagement.DTO;
using StudentManagement.Model;
using StudentManagement.Respository.IRepository;
using StudentManagement.Services.IServices;
using System;
using System.Collections.Generic;
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
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto), "Student data is required.");

                var student = new Student
                {
                    Name = dto.Name,
                    Age = dto.Age,
                    Email = dto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
                };

                await _repo.AddAsync(student);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add student: {ex.Message}", ex);
            }
        }

        public async Task DeleteStudentAsync(int id)
        {
            try
            {
                var student = await _repo.GetByIdAsync(id);
                if (student == null)
                    throw new KeyNotFoundException($"Student with ID {id} not found.");

                await _repo.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete student: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            try
            {
                return await _repo.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve student list.", ex);
            }
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            try
            {
                var student = await _repo.GetByIdAsync(id);
                if (student == null)
                    throw new KeyNotFoundException($"Student with ID {id} not found.");

                return student;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve student: {ex.Message}", ex);
            }
        }

        public async Task UpdateStudentAsync(int id, StudentDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto), "Student data is required.");

                var student = await _repo.GetByIdAsync(id);
                if (student == null)
                    throw new KeyNotFoundException($"Student with ID {id} not found.");

                student.Name = dto.Name;
                student.Age = dto.Age;
                student.Email = dto.Email;

                if (!string.IsNullOrWhiteSpace(dto.Password))
                {
                    student.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                }

                await _repo.UpdateAsync(student);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update student: {ex.Message}", ex);
            }
        }
    }
}
