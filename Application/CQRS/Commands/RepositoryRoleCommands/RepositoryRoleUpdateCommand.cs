using Application.CQRS.Results;
using Application.CQRS.Results.RepositoryRoleResults;
using Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Commands.RepositoryRoleCommands
{
    public class RepositoryRoleUpdateCommand : IRequest<Result<RepositoryRoleUpdateCommandResult>>
    {
        public int RepositoryRoleId { get; set; }
        public RoleValues RoleValue { get; set; }
    }
}
