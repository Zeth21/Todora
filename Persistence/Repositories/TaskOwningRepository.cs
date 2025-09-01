using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class TaskOwningRepository : BaseRepository<TaskOwning>, ITaskOwningRepository
    {
        public TaskOwningRepository(ApplicationDbContext context):base(context)
        {
            
        }
    }
}
