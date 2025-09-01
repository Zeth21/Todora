using Application.Interfaces.UnitOfWork;
using Application.Interfaces.Repositories;
using Persistence.Repositories;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        // Declaring a private and nullable field for every repository.
        // This is for repositories are generated when necessary
        private INotificationRepository? _notificationRepository;
        private IRepositoryRepository? _repositoryRepository;
        private IRepositoryRoleRepository? _repositoryRoleRepository;
        private IRoleRepository? _roleRepository;
        private IStageNoteRepository? _stageNoteRepository;
        private IStageRepository? _stageRepository;
        private ITaskOwningRepository? _taskOwningRepository;
        private ITaskStageRepository? _taskStageRepository;
        private IWorkTaskRepository? _workTaskRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        // Implementing the properties in interfaces with Lazy Loading.
        public INotificationRepository Notifications => _notificationRepository ??= new NotificationRepository(_context);
        public IRepositoryRepository Repositories => _repositoryRepository ??= new RepositoryRepository(_context);
        public IRepositoryRoleRepository RepositoryRoles => _repositoryRoleRepository ??= new RepositoryRoleRepository(_context);
        public IRoleRepository Roles => _roleRepository ??= new RoleRepository(_context);
        public IStageNoteRepository StageNotes => _stageNoteRepository ??= new StageNoteRepository(_context);
        public IStageRepository Stages => _stageRepository ??= new StageRepository(_context);
        public ITaskOwningRepository TaskOwnings => _taskOwningRepository ??= new TaskOwningRepository(_context);
        public ITaskStageRepository TaskStages => _taskStageRepository ??= new TaskStageRepository(_context);
        public IWorkTaskRepository WorkTasks => _workTaskRepository ??= new WorkTaskRepository(_context);

        // This function will return the count of effected rows on the database
        // With this value we can check if the changes are saved.
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Releases the sources that dbContext is using
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}