using Application.CQRS.Commands.RepositoryRoleCommands;
using Application.CQRS.Results;
using Application.CQRS.Results.RepositoryRoleResults;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Handlers.RepositoryRoleHandlers
{
    public class RepositoryRoleDeleteCommandHandler : IRequestHandler<RepositoryRoleDeleteCommand, Result<RepositoryRoleDeleteCommandResult>>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        public RepositoryRoleDeleteCommandHandler(
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        Task<Result<RepositoryRoleDeleteCommandResult>> IRequestHandler<RepositoryRoleDeleteCommand, Result<RepositoryRoleDeleteCommandResult>>.Handle(RepositoryRoleDeleteCommand request, CancellationToken cancellationToken)
        {

            throw new NotImplementedException();
        }
    }
}
