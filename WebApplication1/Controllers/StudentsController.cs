using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces.StudentsInterfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;
        public StudentsController(IStudentService service) => _service = service;

        [HttpPost("filter")]
        public async Task<IActionResult> GetStudents([FromBody] StudentFilter filter, CancellationToken ct) =>
            Ok(await _service.GetStudentsAsync(filter, ct));

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] StudentDto dto, CancellationToken ct)
        {
            await _service.AddStudentAsync(dto, ct);
            return Ok("Студент добавлен");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentDto dto, CancellationToken ct)
        {
            await _service.UpdateStudentAsync(id, dto, ct);
            return Ok("Данные студента обновлены");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id, CancellationToken ct)
        {
            await _service.DeleteStudentAsync(id, ct);
            return Ok("Студент помечен как удаленный");
        }
    }
}
