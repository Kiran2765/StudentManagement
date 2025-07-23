using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.DTO;
using StudentManagement.Services.IServices;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }

        // ✅ Accessible by both Principal and Student
        [Authorize(Roles = "Principal,Student")]
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAll()
        {
            var students = await _service.GetAllStudentsAsync();
            return Ok(students);
        }

        // ✅ Accessible by Principal only
        [Authorize(Roles = "Principal")]
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _service.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound($"Student with ID {id} not found.");

            return Ok(student);
        }

        // ✅ Create - Principal only
        [Authorize(Roles = "Principal")]
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromBody] StudentDto dto)
        {
            await _service.AddStudentAsync(dto);
            return StatusCode(201, "✅ Student created successfully.");
        }

        // ✅ Update - Principal only
        [Authorize(Roles = "Principal")]
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] StudentDto dto)
        {
            var existing = await _service.GetStudentByIdAsync(id);
            if (existing == null)
                return NotFound($"Student with ID {id} not found.");

            await _service.UpdateStudentAsync(id, dto);
            return Ok("✅ Student updated successfully.");
        }

        // ✅ Delete - Principal only
        [Authorize(Roles = "Principal")]
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetStudentByIdAsync(id);
            if (existing == null)
                return NotFound($"Student with ID {id} not found.");

            await _service.DeleteStudentAsync(id);
            return Ok("🗑️ Student deleted successfully.");
        }
    }
}
