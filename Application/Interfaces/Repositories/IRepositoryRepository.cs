using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IRepositoryRepository : IBaseRepository<Repository>
    {
        Task<Repository?> GetUserRepositoryByTitleAsync(string userId, string title);
        Task<(List<Repository> repositories, int totalCount)> GetRepositories(string userId, RepositoryRelationType type, int pageSize, int pageNumber);
    }
}
