using Application.CQRS.Commands.RepositoryRoleCommands;
using Application.CQRS.Results;
using Application.CQRS.Results.RepositoryRoleResults;
using Application.Exceptions;
using Application.Interfaces.Data.Security;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using Domain.Values;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Handlers.RepositoryRoleHandlers
{
    public class RepositoryRoleUpdateCommandHandler : IRequestHandler<RepositoryRoleUpdateCommand, Result<RepositoryRoleUpdateCommandResult>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        public RepositoryRoleUpdateCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IAuthorizationService authorizationService,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }
        public async Task<Result<RepositoryRoleUpdateCommandResult>> Handle(RepositoryRoleUpdateCommand request, CancellationToken cancellationToken)
        {

            var checkRole = await _unitOfWork.RepositoryRoles.GetById(request.RepositoryRoleId);

            if (checkRole == null)
                return Result<RepositoryRoleUpdateCommandResult>.Fail(StringValues.InvalidFail);

            if (checkRole.RepositoryRoleId == (int)request.RoleValue)
                return Result<RepositoryRoleUpdateCommandResult>.Fail(StringValues.CreateFailHasRecord);

            var authUser = _contextAccessor.HttpContext.User;
            var isUserAuthorized = await _authorizationService.AuthorizeAsync(
                authUser,
                checkRole,
                Operations.Update);

            if (!isUserAuthorized.Succeeded)
                return Result<RepositoryRoleUpdateCommandResult>.Unauthorized();

            checkRole.RoleId = (int)request.RoleValue;
            await _unitOfWork.RepositoryRoles.UpdateAsync(checkRole);
            var affectedRows = await _unitOfWork.CompleteAsync();

            if (affectedRows <= 0)
                throw new SaveDataException(StringValues.SaveFail, new Exception());

            var record = await _unitOfWork.RepositoryRoles.FindUserRepositoryRole(checkRole.UserId, checkRole.RepositoryId);
            var result = _mapper.Map<RepositoryRoleUpdateCommandResult>(record);

            return Result<RepositoryRoleUpdateCommandResult>.Success(result,IntegerValues.Ok,StringValues.CreateSuccess);
        }
    }
}
