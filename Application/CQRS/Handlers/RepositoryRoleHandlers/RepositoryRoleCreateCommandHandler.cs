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

namespace Application.CQRS.Handlers.RepositoryRoleHandlers
{
    public class RepositoryRoleCreateCommandHandler : IRequestHandler<RepositoryRoleCreateCommand, Result<RepositoryRoleCreateCommandResult>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RepositoryRoleCreateCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            UserManager<User> userManager,
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Result<RepositoryRoleCreateCommandResult>> Handle(RepositoryRoleCreateCommand request, CancellationToken cancellationToken)
        {
            var checkUser = await _userManager.FindByIdAsync(request.UserId);
            if (checkUser == null)
                return Result<RepositoryRoleCreateCommandResult>.Fail(message: StringValues.InvalidUser);

            var checkRepository = await _unitOfWork.Repositories.GetById(request.RepositoryId);
            if (checkRepository == null)
                return Result<RepositoryRoleCreateCommandResult>.Fail(message: StringValues.InvalidRepository);

            var authUser = _httpContextAccessor.HttpContext.User;
            var authorizationResult = await _authorizationService.AuthorizeAsync(
                authUser,           
                checkRepository,    
                Operations.Create  
            );

            if (!authorizationResult.Succeeded)
            {
                return Result<RepositoryRoleCreateCommandResult>.Forbidden();
            }


            var checkRepositoryRole = await _unitOfWork.RepositoryRoles.FindUserRepositoryRole(request.UserId, request.RepositoryId);
            if (checkRepositoryRole is not null)
                return Result<RepositoryRoleCreateCommandResult>.Fail(message: StringValues.CreateFailHasRecord);

            var newRecord = _mapper.Map<RepositoryRole>(request);
            await _unitOfWork.RepositoryRoles.AddAsync(newRecord);
            var affectedRows = await _unitOfWork.CompleteAsync();

            if (affectedRows <= 0)
                throw new SaveDataException(StringValues.SaveFail, new Exception());

            var record = await _unitOfWork.RepositoryRoles.FindUserRepositoryRole(request.UserId, request.RepositoryId);
            var result = _mapper.Map<RepositoryRoleCreateCommandResult>(record);
            return Result<RepositoryRoleCreateCommandResult>.Success(data: result, IntegerValues.Created, message: StringValues.CreateSuccess);
        }
    }
}