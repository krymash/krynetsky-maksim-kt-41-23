using WebApplication1.Interfaces;
using WebApplication1.Interfaces.StudentsInterfaces;

namespace WebApplication1.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IDisciplineService, DisciplineService>();
            services.AddScoped<IGradeService, GradeService>();

            return services;
        }
    }
}
