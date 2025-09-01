using Application.Interfaces.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.ServiceRegistration
{
    public static class RepositoryServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {


            //services.AddDbContext<ApplicationDbContext>();
            //services.AddScoped<INotificationRepository, NotificationRepository>();
            //services.AddScoped<IRepositoryRepository, RepositoryRepository>();
            //services.AddScoped<IRepositoryRoleRepository, RepositoryRoleRepository>();
            //services.AddScoped<IRoleRepository, RoleRepository>();
            //services.AddScoped<IStageNoteRepository, StageNoteRepository>();
            //services.AddScoped<IStageRepository, StageRepository>();
            //services.AddScoped<ITaskOwningRepository, TaskOwningRepository>();
            //services.AddScoped<ITaskStageRepository, TaskStageRepository>();
            //services.AddScoped<IWorkTaskRepository, WorkTaskRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
