using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class RepositoryRoleRepository : BaseRepository<RepositoryRole>, IRepositoryRoleRepository
    {
        public RepositoryRoleRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<RepositoryRole?> FindUserRepositoryRole(string userId, int repositoryId) 
        {
            var repositoryRole = await _context.RepositoryRoles
                .Include(x => x.Role)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.RepositoryId == repositoryId);
            return repositoryRole;
        }
    }
}
