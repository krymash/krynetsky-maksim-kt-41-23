using Microsoft.EntityFrameworkCore;
using WebApplication1.Database;
using WebApplication1.Filters.StudentFilters;
using WebApplication1.Interfaces.StudentsInterfaces;
using WebApplication1.Models;

namespace KrynetskyMaksimKt_41_23.Tests
{
    public class StudentIntegrationTests
    {
        public readonly DbContextOptions<StudentDbContext> _dbContextOptions;

        public StudentIntegrationTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentDbContext>()
                .UseInMemoryDatabase(databaseName: "student_db")
                .Options;
        }

        [Fact]
        public async Task GetStudentsByGroupAsync_KT4123_TwoObjects()
        {
            var ctx = new StudentDbContext(_dbContextOptions);
            var studentService = new StudentService(ctx);

            var groups = new List<Group>
            {
                new Group
                {
                    Name = "КТ-41-23"
                },
                new Group
                {
                    Name = "КТ-42-23"
                }
            };
            await ctx.Set<Group>().AddRangeAsync(groups);
            await ctx.SaveChangesAsync();

            var students = new List<Student>
            {
                new Student
                {
                    FirstName = "Иван",
                    LastName = "Иванов",
                    GroupId = 1,
                },
                new Student
                {
                    FirstName = "Петр",
                    LastName = "Петров",
                    GroupId = 2,
                },
                new Student
                {
                    FirstName = "Сидор",
                    LastName = "Сидоров",
                    GroupId = 1,
                }
            };
            await ctx.Set<Student>().AddRangeAsync(students);
            await ctx.SaveChangesAsync();

            var filter = new StudentGroupFilter
            {
                GroupName = "КТ-41-23"
            };
            var studentsResult = await studentService.GetStudentsByGroupAsync(filter, CancellationToken.None);

            Assert.Equal(2, studentsResult.Length);
        }
    }
}
