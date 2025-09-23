using Application.CQRS.Results;
using Application.CQRS.Results.RepositoryRoleResults;
using Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.CQRS.Commands.RepositoryRoleCommands
{
    public class RepositoryRoleCreateCommand : IRequest<Result<RepositoryRoleCreateCommandResult>>
    {
        [JsonIgnore]
        public string AuthorizedPersonId { get; set; } = string.Empty;
        public int RepositoryId { get; set; }
        public string UserId { get; set; } = null!;
        public RoleValues RoleName { get; set; }
    }
}
