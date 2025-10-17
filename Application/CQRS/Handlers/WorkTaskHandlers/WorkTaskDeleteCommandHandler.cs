using Application.CQRS.Commands.WorkTaskCommands;
using Application.CQRS.Results;
using Application.CQRS.Results.WorkTaskResults;
using Application.Interfaces.Data.Security;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Handlers.WorkTaskHandlers
{
    public class WorkTaskDeleteCommandHandler : IRequestHandler<WorkTaskDeleteCommand, Result<WorkTaskDeleteCommandResult>>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        public WorkTaskDeleteCommandHandler(
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork
            ) 
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<WorkTaskDeleteCommandResult>> Handle(WorkTaskDeleteCommand request, CancellationToken cancellationToken)
        {
            var workTask = await _unitOfWork.WorkTasks.GetById(request.WorkTaskId);
            if (workTask == null)
                return Result<WorkTaskDeleteCommandResult>.NotFound();

            var authorizedUser = _httpContextAccessor.HttpContext.User;
            if(authorizedUser is null)
                return Result<WorkTaskDeleteCommandResult>.Unauthorized();

            var authorizationResult = await _authorizationService
                .AuthorizeAsync(
                authorizedUser,
                workTask,
                Operations.Delete
                );
            
            if(!authorizationResult.Succeeded)
                return Result<WorkTaskDeleteCommandResult>.Forbidden();

            workTask.TaskIsRemoved = true;
            await _unitOfWork.CompleteAsync();
            
            return Result<WorkTaskDeleteCommandResult>.NoContent();
        }
    }
}
