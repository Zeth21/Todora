using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enum;
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

        async Task<(List<Repository> repositories, int totalCount)> IRepositoryRepository.GetRepositories(string userId, RepositoryRelationType type, int pageSize, int pageNumber)
        {
            var query = _context
                .Repositories
                .AsQueryable()
                .AsNoTracking();
            switch (type) 
            {
                case RepositoryRelationType.All:
                    query = query
                        .Where(x => x.RepositoryUserId == userId || x.RepositoryRoles.Any(rr => rr.UserId == userId));
                    break;
                case RepositoryRelationType.Owner:
                    query = query
                        .Where(x => x.RepositoryUserId == userId);
                    break;
                case RepositoryRelationType.Working:
                    query = query
                        .Where(x => x.RepositoryRoles.Any(rr => rr.UserId == userId) && x.RepositoryUserId != userId);
                    break;
                default:
                    break;
            };
            var totalcount = await query.CountAsync();

            var repositories = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (repositories, totalcount);
        }
    }
}
