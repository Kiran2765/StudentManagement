using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Model;
using StudentManagement.Models;
using StudentManagement.Respository.IRepository;
using System.Threading.Tasks;

namespace StudentManagement.Respository
{
    public class PrincipalRepository : IPrincipalRepository
    {
        private readonly ApplicationDbContext _context;

        public PrincipalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddPrincipalAsync(Principal principal)
        {
            _context.Principals.Add(principal);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> PrincipalExistsAsync(string email)
        {
            return await _context.Principals.AnyAsync(p => p.Email == email);
        }

        public async Task<Principal> GetByEmailAsync(string email)
        {
            return await _context.Principals.FirstOrDefaultAsync(p => p.Email == email);
        }
    }
}
