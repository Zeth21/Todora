using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetById(int id);
        void DeleteById(int id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
    }
}
