using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _service;
        public GroupsController(IGroupService service) => _service = service;

        [HttpPost("filter")]
        public async Task<IActionResult> GetGroups([FromBody] GroupFilter filter, CancellationToken ct) =>
            Ok(await _service.GetGroupsAsync(filter, ct));

        [HttpPost]
        public async Task<IActionResult> AddGroup([FromBody] GroupDto dto, CancellationToken ct)
        {
            await _service.AddGroupAsync(dto, ct);
            return Ok("Группа добавлена");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] GroupDto dto, CancellationToken ct)
        {
            await _service.UpdateGroupAsync(id, dto, ct);
            return Ok("Группа обновлена");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id, CancellationToken ct)
        {
            await _service.DeleteGroupAsync(id, ct);
            return Ok("Группа и её студенты помечены как удаленные");
        }
    }
}
