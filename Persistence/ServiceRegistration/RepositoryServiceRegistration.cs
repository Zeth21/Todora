using Application.Interfaces.Data.Security;
using Application.Interfaces.File;
using Application.Interfaces.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data.File;
using Persistence.Data.Security;

namespace Persistence.ServiceRegistration
{
    public static class RepositoryServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {


            services.AddDbContext<ApplicationDbContext>();
            services.AddTransient<IJWTGenerator, JWTGenerator>();
            services.AddScoped<IRepositoryContextResolver, RepositoryContextResolver>();
            services.AddTransient<IFileService, FileService>();

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
