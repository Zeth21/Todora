using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class WorkTaskRepository : BaseRepository<WorkTask>, IWorkTaskRepository
    {
        public WorkTaskRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<bool> CheckTitleIsValid(int repositoryId, string title)
        {
            var isValid = await _context.Tasks
                .AnyAsync(x => x.RepositoryId == repositoryId && x.TaskTitle == title);
            return isValid;
        }
    }
}
