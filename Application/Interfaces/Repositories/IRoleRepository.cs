using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<List<Role>?> GetAllRoles();
        Task<Role?> GetRoleByValue(RoleValues roleValue);
    }
}
