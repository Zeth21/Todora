using Application.CQRS.Results;
using Application.CQRS.Results.RepositoryRoleResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Commands.RepositoryRoleCommands
{
    public class RepositoryRoleDeleteCommand : IRequest<Result<RepositoryRoleDeleteCommandResult>>
    {
        public int RepositoryRoleId { get; set; }
    }
}
