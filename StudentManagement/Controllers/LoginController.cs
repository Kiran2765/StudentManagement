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
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid login request");

            var token = await _loginService.LoginAsync(dto);
            if (token == null)
                return Unauthorized("❌ Invalid email or password");

            // Return token (or optionally set a secure cookie if needed)
            return Ok(new
            {
                Token = token,
                Message = "✅ Login successful"
            });
        }
    }
}
