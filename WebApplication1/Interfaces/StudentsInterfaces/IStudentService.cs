using Microsoft.EntityFrameworkCore;
using WebApplication1.Database;
using WebApplication1.Filters.StudentFilters;
using WebApplication1.Models;

namespace WebApplication1.Interfaces.StudentsInterfaces
{
    public class StudentFilter
    {
        public string? GroupName { get; set; }
        public string? FullName { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class StudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GroupId { get; set; }
    }

    public interface IStudentService
    {
        // Старый метод (оставлен для работы вашего теста)
        Task<Student[]> GetStudentsByGroupAsync(StudentGroupFilter filter, CancellationToken cancellationToken);

        // Новые методы по заданию (пункты 2 и 6)
        Task<Student[]> GetStudentsAsync(StudentFilter filter, CancellationToken ct = default);
        Task AddStudentAsync(StudentDto dto, CancellationToken ct = default);
        Task UpdateStudentAsync(int id, StudentDto dto, CancellationToken ct = default);
        Task DeleteStudentAsync(int id, CancellationToken ct = default);
    }

    public class StudentService : IStudentService
    {
        private readonly StudentDbContext _dbContext;
        public StudentService(StudentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Старый метод
        public Task<Student[]> GetStudentsByGroupAsync(StudentGroupFilter filter, CancellationToken cancellationToken = default)
        {
            var students = _dbContext.Set<Student>()
                .Where(w => w.Group.Name == filter.GroupName)
                .ToArrayAsync(cancellationToken);

            return students;
        }

        // 2) Получение списка студентов с фильтрацией по группе, ФИО и статусу удаления
        public async Task<Student[]> GetStudentsAsync(StudentFilter filter, CancellationToken ct = default)
        {
            var query = _dbContext.Set<Student>().Include(s => s.Group).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.GroupName))
                query = query.Where(s => s.Group != null && s.Group.Name == filter.GroupName);

            if (!string.IsNullOrWhiteSpace(filter.FullName))
            {
                var search = filter.FullName.ToLower();
                query = query.Where(s => (s.LastName + " " + s.FirstName).ToLower().Contains(search));
            }

            if (filter.IsDeleted.HasValue)
                query = query.Where(s => s.IsDeleted == filter.IsDeleted.Value);

            return await query.ToArrayAsync(ct);
        }

        // 6) Добавление студента
        public async Task AddStudentAsync(StudentDto dto, CancellationToken ct = default)
        {
            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                GroupId = dto.GroupId,
                IsDeleted = false
            };
            await _dbContext.Set<Student>().AddAsync(student, ct);
            await _dbContext.SaveChangesAsync(ct);
        }

        // 6) Изменение студента
        public async Task UpdateStudentAsync(int id, StudentDto dto, CancellationToken ct = default)
        {
            var student = await _dbContext.Set<Student>().FindAsync(new object[] { id }, ct)
                ?? throw new Exception("Студент не найден");

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.GroupId = dto.GroupId;

            await _dbContext.SaveChangesAsync(ct);
        }

        // 6) Мягкое удаление студента
        public async Task DeleteStudentAsync(int id, CancellationToken ct = default)
        {
            var student = await _dbContext.Set<Student>().FindAsync(new object[] { id }, ct)
                ?? throw new Exception("Студент не найден");

            student.IsDeleted = true;
            await _dbContext.SaveChangesAsync(ct);
        }
    }
}
