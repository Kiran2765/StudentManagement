using BCrypt.Net;
using StudentManagement.DTO;
using StudentManagement.Model;
using StudentManagement.Models;
using StudentManagement.Respository.IRepository;
using StudentManagement.Services.IServices;

namespace StudentManagement.Services
{
    public class PrincipalService : IPrincipalService
    {
        private readonly IPrincipalRepository _repo;

        public PrincipalService(IPrincipalRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> RegisterPrincipalAsync(PrincipalDto dto)
        {
            if (await _repo.PrincipalExistsAsync(dto.Email))
                return false;

            var principal = new Principal
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _repo.AddPrincipalAsync(principal);
            return true;
        }
    }
}
