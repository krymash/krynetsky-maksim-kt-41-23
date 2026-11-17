using Microsoft.EntityFrameworkCore;
using WebApplication1.Database;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public class GroupFilter
    {
        public string? SpecialityTitle { get; set; }
        public int? Course { get; set; }
        public int? YearSuffix { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class GroupDto
    {
        public string Name { get; set; }
        public int Course { get; set; }
        public int SpecialityId { get; set; }
    }

    public interface IGroupService
    {
        Task<Group[]> GetGroupsAsync(GroupFilter filter, CancellationToken ct = default);
        Task AddGroupAsync(GroupDto dto, CancellationToken ct = default);
        Task UpdateGroupAsync(int id, GroupDto dto, CancellationToken ct = default);
        Task DeleteGroupAsync(int id, CancellationToken ct = default);
    }

    public class GroupService : IGroupService
    {
        private readonly StudentDbContext _db;
        public GroupService(StudentDbContext db) => _db = db;

        public async Task<Group[]> GetGroupsAsync(GroupFilter filter, CancellationToken ct = default)
        {
            var query = _db.Set<Group>().Include(g => g.Speciality).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SpecialityTitle))
                query = query.Where(g => g.Speciality != null && g.Speciality.Title.Contains(filter.SpecialityTitle));

            if (filter.Course.HasValue)
                query = query.Where(g => g.Course == filter.Course.Value);

            if (filter.YearSuffix.HasValue)
                query = query.Where(g => g.Name.EndsWith($"-{filter.YearSuffix.Value}"));

            if (filter.IsDeleted.HasValue)
                query = query.Where(g => g.IsDeleted == filter.IsDeleted.Value);

            return await query.ToArrayAsync(ct);
        }

        public async Task AddGroupAsync(GroupDto dto, CancellationToken ct = default)
        {
            var group = new Group
            {
                Name = dto.Name,
                Course = dto.Course,
                SpecialityId = dto.SpecialityId,
                IsDeleted = false
            };
            await _db.Set<Group>().AddAsync(group, ct);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateGroupAsync(int id, GroupDto dto, CancellationToken ct = default)
        {
            var group = await _db.Set<Group>().FindAsync(new object[] { id }, ct)
                ?? throw new Exception("Группа не найдена");

            group.Name = dto.Name;
            group.Course = dto.Course;
            group.SpecialityId = dto.SpecialityId;

            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteGroupAsync(int id, CancellationToken ct = default)
        {
            var group = await _db.Set<Group>()
                .Include(g => g.Students)
                .FirstOrDefaultAsync(g => g.GroupId == id, ct)
                ?? throw new Exception("Группа не найдена");

            group.IsDeleted = true;

            if (group.Students != null)
            {
                foreach (var student in group.Students)
                {
                    student.IsDeleted = true;
                }
            }

            await _db.SaveChangesAsync(ct);
        }
    }
}
