using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _service;
        public GradesController(IGradeService service) => _service = service;

        [HttpGet("average-by-group")]
        public async Task<IActionResult> GetAverageByGroup(string groupName, string disciplineName, CancellationToken ct) =>
            Ok(await _service.GetAverageGradeByGroupAndDisciplineAsync(groupName, disciplineName, ct));

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentGrades(int studentId, CancellationToken ct) =>
            Ok(await _service.GetStudentGradesAsync(studentId, ct));

        [HttpGet("average-by-year/{yearSuffix}")]
        public async Task<IActionResult> GetAverageByYear(int yearSuffix, CancellationToken ct) =>
            Ok(await _service.GetAverageGradeByYearAsync(yearSuffix, ct));

        [HttpPost]
        public async Task<IActionResult> AddGrade([FromBody] GradeDto dto, CancellationToken ct)
        {
            await _service.AddGradeAsync(dto, ct);
            return Ok("Оценка добавлена");
        }

        [HttpPut("{gradeId}")]
        public async Task<IActionResult> UpdateGrade(int gradeId, [FromBody] int newValue, CancellationToken ct)
        {
            await _service.UpdateGradeAsync(gradeId, newValue, ct);
            return Ok("Оценка обновлена");
        }
    }
}
