using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class TaskStageRepository : BaseRepository<TaskStage>, ITaskStageRepository
    {
        public TaskStageRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
