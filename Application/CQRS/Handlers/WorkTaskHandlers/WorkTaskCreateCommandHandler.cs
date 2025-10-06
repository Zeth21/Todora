using Application.CQRS.Commands.WorkTaskCommands;
using Application.CQRS.Results;
using Application.CQRS.Results.WorkTaskResults;
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

namespace Application.CQRS.Handlers.WorkTaskHandlers
{
    public class WorkTaskCreateCommandHandler : IRequestHandler<WorkTaskCreateCommand, Result<WorkTaskCreateCommandResult>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WorkTaskCreateCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<WorkTaskCreateCommandResult>> Handle(WorkTaskCreateCommand request, CancellationToken cancellationToken)
        {
            var checkRepository = await _unitOfWork.Repositories.GetById(request.RepositoryId);
            if (checkRepository == null)
                return Result<WorkTaskCreateCommandResult>.Fail(StringValues.InvalidRepository);

            var titleHasTaken = await _unitOfWork.WorkTasks.CheckTitleIsValid(checkRepository.RepositoryId, request.TaskTitle);
            if(titleHasTaken)
                return Result<WorkTaskCreateCommandResult>.Fail(StringValues.InvalidTitleSame);

            var policyResource = _mapper.Map<WorkTask>(request);

            var authUser = _httpContextAccessor.HttpContext.User;
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                authUser,
                policyResource,
                Operations.Create);
            if (!isAuthorized.Succeeded)
                return Result<WorkTaskCreateCommandResult>.Unauthorized();

            var newRecord = _mapper.Map<WorkTask>(request);
            await _unitOfWork.WorkTasks.AddAsync(newRecord);
            var affectedRows = await _unitOfWork.CompleteAsync();
            if (affectedRows <= 0)
                throw new SaveDataException(StringValues.SaveFail, new Exception());

            var result = _mapper.Map<WorkTaskCreateCommandResult>(newRecord);
            return Result<WorkTaskCreateCommandResult>.Success(result,IntegerValues.Created,StringValues.CreateSuccess);
        }
    }
}
