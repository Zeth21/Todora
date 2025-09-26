using Domain.Enum;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Data.Security
{
    public interface IAuthorizationRules
    {
        bool IsAuthorized(RoleValues userRole, ProjectOperationRequirement requirement, object resource);
    }
}
