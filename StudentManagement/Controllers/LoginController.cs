using Microsoft.AspNetCore.Mvc;
using StudentManagement.DTO;
using StudentManagement.Services.IServices;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var student = await _loginService.LoginAsync(dto);
            if (student != null)
            {
                return Ok(new
                {
                    Message = "Login successful",
                    student.Id,
                    student.Name,
                    student.Email
                });
            }
            return Unauthorized("Invalid email or password.");
        }
    }
}
