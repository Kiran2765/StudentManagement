using Microsoft.AspNetCore.Mvc;
using StudentManagement.DTO;
using StudentManagement.Services.IServices;
using StudentManagement.DTO;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentController(IStudentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllStudentsAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _service.GetStudentByIdAsync(id);
        return student != null ? Ok(student) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StudentDto dto)
    {
        await _service.AddStudentAsync(dto);
        return Ok("Student created.");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] StudentDto dto)
    {
        await _service.UpdateStudentAsync(id, dto);
        return Ok("Student updated.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteStudentAsync(id);
        return Ok("Student deleted.");
    }
}
