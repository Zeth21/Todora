using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Results.RepositoryRoleResults
{
    public class RepositoryRoleCreateCommandResult
    {
        public int RepositoryRoleId { get; set; }
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public RoleValues UserRepositoryRole { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
