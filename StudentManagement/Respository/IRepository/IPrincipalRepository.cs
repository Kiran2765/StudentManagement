using StudentManagement.Models;

namespace StudentManagement.Respository.IRepository
{
    public interface IPrincipalRepository
    {
        Task AddPrincipalAsync(Principal principal);
        Task<bool> PrincipalExistsAsync(string email);
        Task<Principal> GetByEmailAsync(string email);
    }
}
