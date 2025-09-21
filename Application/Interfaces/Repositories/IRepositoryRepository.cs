using Domain.Entities;
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
        Task<List<Repository>> GetUserOwningRepositoriesByUserId(string userId);
        Task<List<Repository>> GetUserWorkingRepositoriesByUserId(string userId);
    }
}
