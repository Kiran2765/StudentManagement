using StudentManagement.DTO;

namespace StudentManagement.Services.IServices
{
    public interface IPrincipalService
    {
        Task<bool> RegisterPrincipalAsync(PrincipalDto dto);
    }
}
