using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Results.RoleResults
{
    public class RoleGetAllQueryResult
    {
        public int Id { get; set; }
        public RoleValues RoleName { get; set; }
    }
}
