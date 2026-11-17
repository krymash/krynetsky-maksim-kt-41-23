using Microsoft.EntityFrameworkCore;
using WebApplication1.Database;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public class GradeDto
    {
        public int StudentId { get; set; }
        public int DisciplineId { get; set; }
        public int Value { get; set; }
    }

    public class StudentGradeResponse
    {
        public string StudentFullName { get; set; }
        public string DisciplineName { get; set; }
        public int GradeValue { get; set; }
    }

    public interface IGradeService
    {
        Task<double> GetAverageGradeByGroupAndDisciplineAsync(string groupName, string disciplineName, CancellationToken ct = default);
        Task<StudentGradeResponse[]> GetStudentGradesAsync(int studentId, CancellationToken ct = default);
        Task<double> GetAverageGradeByYearAsync(int yearSuffix, CancellationToken ct = default);
        Task AddGradeAsync(GradeDto dto, CancellationToken ct = default);
        Task UpdateGradeAsync(int gradeId, int newValue, CancellationToken ct = default);
    }

    public class GradeService : IGradeService
    {
        private readonly StudentDbContext _db;
        public GradeService(StudentDbContext db) => _db = db;

        public async Task<double> GetAverageGradeByGroupAndDisciplineAsync(string groupName, string disciplineName, CancellationToken ct = default)
        {
            var grades = await _db.Set<Grade>()
                .Include(g => g.Student).ThenInclude(s => s.Group)
                .Include(g => g.Discipline)
                .Where(g => g.Student != null && g.Student.Group != null && g.Student.Group.Name == groupName
                         && g.Discipline != null && g.Discipline.Name == disciplineName)
                .Select(g => g.Value)
                .ToListAsync(ct);

            return grades.Any() ? Math.Round(grades.Average(), 2) : 0;
        }

        public async Task<StudentGradeResponse[]> GetStudentGradesAsync(int studentId, CancellationToken ct = default)
        {
            return await _db.Set<Grade>()
                .Include(g => g.Student)
                .Include(g => g.Discipline)
                .Where(g => g.StudentId == studentId)
                .Select(g => new StudentGradeResponse
                {
                    StudentFullName = $"{g.Student!.LastName} {g.Student.FirstName}",
                    DisciplineName = g.Discipline!.Name,
                    GradeValue = g.Value
                })
                .ToArrayAsync(ct);
        }

        public async Task<double> GetAverageGradeByYearAsync(int yearSuffix, CancellationToken ct = default)
        {
            var grades = await _db.Set<Grade>()
                .Include(g => g.Student).ThenInclude(s => s.Group)
                .Where(g => g.Student != null && g.Student.Group != null && g.Student.Group.Name.EndsWith($"-{yearSuffix}"))
                .Select(g => g.Value)
                .ToListAsync(ct);

            return grades.Any() ? Math.Round(grades.Average(), 2) : 0;
        }

        public async Task AddGradeAsync(GradeDto dto, CancellationToken ct = default)
        {
            var grade = new Grade
            {
                StudentId = dto.StudentId,
                DisciplineId = dto.DisciplineId,
                Value = dto.Value
            };
            await _db.Set<Grade>().AddAsync(grade, ct);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateGradeAsync(int gradeId, int newValue, CancellationToken ct = default)
        {
            var grade = await _db.Set<Grade>().FindAsync(new object[] { gradeId }, ct)
                ?? throw new Exception("Оценка не найдена");

            grade.Value = newValue;
            await _db.SaveChangesAsync(ct);
        }
    }
}
