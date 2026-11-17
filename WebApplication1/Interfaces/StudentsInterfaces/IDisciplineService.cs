using Microsoft.EntityFrameworkCore;
using WebApplication1.Database;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public class DisciplineFilter
    {
        public string? Direction { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class DisciplineDto
    {
        public string Name { get; set; }
    }

    public interface IDisciplineService
    {
        Task<Discipline[]> GetDisciplinesAsync(DisciplineFilter filter, CancellationToken ct = default);
        Task AddDisciplineAsync(DisciplineDto dto, CancellationToken ct = default);
        Task UpdateDisciplineAsync(int id, DisciplineDto dto, CancellationToken ct = default);
        Task DeleteDisciplineAsync(int id, CancellationToken ct = default);
    }

    public class DisciplineService : IDisciplineService
    {
        private readonly StudentDbContext _db;
        public DisciplineService(StudentDbContext db) => _db = db;

        public async Task<Discipline[]> GetDisciplinesAsync(DisciplineFilter filter, CancellationToken ct = default)
        {
            var query = _db.Set<Discipline>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Direction))
            {
                var humKeywords = new[] { "история", "философия", "право", "экономика", "язык", "психология" };
                var dir = filter.Direction.ToLower();

                if (dir.Contains("гум"))
                {
                    query = query.Where(d => humKeywords.Any(k => d.Name.ToLower().Contains(k)));
                }
                else if (dir.Contains("тех"))
                {
                    query = query.Where(d => !humKeywords.Any(k => d.Name.ToLower().Contains(k)));
                }
            }

            if (filter.IsDeleted.HasValue)
                query = query.Where(d => d.IsDeleted == filter.IsDeleted.Value);

            return await query.ToArrayAsync(ct);
        }

        public async Task AddDisciplineAsync(DisciplineDto dto, CancellationToken ct = default)
        {
            var discipline = new Discipline
            {
                Name = dto.Name,
                IsDeleted = false
            };
            await _db.Set<Discipline>().AddAsync(discipline, ct);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateDisciplineAsync(int id, DisciplineDto dto, CancellationToken ct = default)
        {
            var discipline = await _db.Set<Discipline>().FindAsync(new object[] { id }, ct)
                ?? throw new Exception("Дисциплина не найдена");

            discipline.Name = dto.Name;
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteDisciplineAsync(int id, CancellationToken ct = default)
        {
            var discipline = await _db.Set<Discipline>().FindAsync(new object[] { id }, ct)
                ?? throw new Exception("Дисциплина не найдена");

            discipline.IsDeleted = true;
            await _db.SaveChangesAsync(ct);
        }
    }
}
