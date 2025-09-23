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
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<List<Role>?> GetAllRoles()
        {
            var roles = await _context.Rolles.AsNoTracking().ToListAsync();
            return roles;
        }
    }
}
