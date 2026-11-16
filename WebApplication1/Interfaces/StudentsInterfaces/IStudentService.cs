using WebApplication1.Database;
using WebApplication1.Filters.StudentFilters;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Interfaces.StudentsInterfaces
{
    public interface IStudentService
    {
        public Task<Student[]> GetStudentsByGroupAsync(StudentGroupFilter filter, CancellationToken cancellationToken);
    }

    public class StudentService : IStudentService
    {
        private readonly StudentDbContext _dbContext;
        public StudentService(StudentDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<Student[]> GetStudentsByGroupAsync(StudentGroupFilter filter, CancellationToken cancellationToken = default)
        {
            var students = _dbContext.Set<Student>().Where(w => w.Group.Name == filter.GroupName).ToArrayAsync(cancellationToken);

            return students;
        }
    }
}
