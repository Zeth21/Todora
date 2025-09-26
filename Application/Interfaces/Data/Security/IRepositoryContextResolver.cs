using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Data.Security
{
    public interface IRepositoryContextResolver
    {
        Task<int?> GetRepositoryIdForResourceAsync(object resource);
    }
}
