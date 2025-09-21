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
    public class RepositoryRepository : BaseRepository<Repository>, IRepositoryRepository
    {
        public RepositoryRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<Repository?> GetUserRepositoryByTitleAsync(string userId, string title)
        {
            var repository = await _context
                .Repositories
                .Where(x => x.RepositoryUserId == userId && x.RepositoryTitle == title)
                .FirstOrDefaultAsync();
            return repository;
        }

        async Task<List<Repository>> IRepositoryRepository.GetUserOwningRepositoriesByUserId(string userId)
        {
            var repositories = await _context
                .Repositories
                .AsNoTracking()
                .Where(x => x.RepositoryUserId == userId)
                .ToListAsync();
            if (!repositories.Any())
                repositories = new List<Repository>();
            return repositories;
        }

        async Task<List<Repository>> IRepositoryRepository.GetUserWorkingRepositoriesByUserId(string userId)
        {
            var repositories = await _context
                .Repositories
                .Include(x => x.RepositoryRoles)
                .AsNoTracking()
                .Where(x => x.RepositoryRoles.Where(x => x.UserId == userId).Select(x => x.RepositoryId).Contains(x.RepositoryId))
                .Select(x => x)
                .ToListAsync();
            if (!repositories.Any())
                repositories = new List<Repository>();
            return repositories;
        }
    }
}
