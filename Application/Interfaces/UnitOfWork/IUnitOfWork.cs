using Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        INotificationRepository Notifications { get; }
        IRepositoryRepository Repositories { get; }
        IRepositoryRoleRepository RepositoryRoles { get; }
        IRoleRepository Roles { get; }
        IStageNoteRepository StageNotes { get; }
        IStageRepository Stages { get; }
        ITaskOwningRepository TaskOwnings { get; } 
        ITaskStageRepository TaskStages { get; }
        IWorkTaskRepository WorkTasks { get; }
        Task<int> CompleteAsync();
    }
}
