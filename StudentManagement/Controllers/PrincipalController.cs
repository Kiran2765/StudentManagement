using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.DTO;
using StudentManagement.Services.IServices;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrincipalController : ControllerBase
    {
        private readonly IPrincipalService _principalService;

        public PrincipalController(IPrincipalService principalService)
        {
            _principalService = principalService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] PrincipalDto dto)
        {
            var result = await _principalService.RegisterPrincipalAsync(dto);
            if (!result)
                return BadRequest("Principal with this email already exists.");

            return Ok("Principal registered successfully.");
        }
    }
}
