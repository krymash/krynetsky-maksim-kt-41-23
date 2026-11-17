using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisciplinesController : ControllerBase
    {
        private readonly IDisciplineService _service;
        public DisciplinesController(IDisciplineService service) => _service = service;

        [HttpPost("filter")]
        public async Task<IActionResult> GetDisciplines([FromBody] DisciplineFilter filter, CancellationToken ct) =>
            Ok(await _service.GetDisciplinesAsync(filter, ct));

        [HttpPost]
        public async Task<IActionResult> AddDiscipline([FromBody] DisciplineDto dto, CancellationToken ct)
        {
            await _service.AddDisciplineAsync(dto, ct);
            return Ok("Дисциплина добавлена");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiscipline(int id, [FromBody] DisciplineDto dto, CancellationToken ct)
        {
            await _service.UpdateDisciplineAsync(id, dto, ct);
            return Ok("Дисциплина обновлена");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscipline(int id, CancellationToken ct)
        {
            await _service.DeleteDisciplineAsync(id, ct);
            return Ok("Дисциплина помечена как удаленная");
        }
    }
}
