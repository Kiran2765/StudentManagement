using StudentManagement.Data;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Model;
using StudentManagement.Respository.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Respository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            try
            {
                return await _context.Students.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve students.", ex);
            }
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                    throw new KeyNotFoundException($"Student with ID {id} not found.");

                return student;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve student by ID: {id}", ex);
            }
        }

        public async Task AddAsync(Student student)
        {
            try
            {
                if (student == null)
                    throw new ArgumentNullException(nameof(student), "Student data cannot be null.");

                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add student.", ex);
            }
        }

        public async Task UpdateAsync(Student student)
        {
            try
            {
                if (student == null)
                    throw new ArgumentNullException(nameof(student), "Student data cannot be null.");

                var existingStudent = await _context.Students.FindAsync(student.Id);
                if (existingStudent == null)
                    throw new KeyNotFoundException($"Student with ID {student.Id} not found.");

                _context.Entry(existingStudent).CurrentValues.SetValues(student);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update student.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                    throw new KeyNotFoundException($"Student with ID {id} not found.");

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete student.", ex);
            }
        }

        // ✅ Updated as per your request: returns null if student not found
        public async Task<Student> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            return await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
        }
    }
}
