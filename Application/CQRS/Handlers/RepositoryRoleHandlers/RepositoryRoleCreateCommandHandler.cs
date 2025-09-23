using Application.CQRS.Commands.RepositoryRoleCommands;
using Application.CQRS.Results;
using Application.CQRS.Results.RepositoryRoleResults;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using Domain.Strings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Handlers.RepositoryRoleHandlers
{
    public class RepositoryRoleCreateCommandHandler : IRequestHandler<RepositoryRoleCreateCommand, Result<RepositoryRoleCreateCommandResult>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RepositoryRoleCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<RepositoryRoleCreateCommandResult>> Handle(RepositoryRoleCreateCommand request, CancellationToken cancellationToken)
        {
            //CHECK USERS AUTH
            var checkRepository = await _unitOfWork.Repositories.GetById(request.RepositoryId);
            if (checkRepository == null) 
            {
                return Result<RepositoryRoleCreateCommandResult>.Fail(message:StringValues.InvalidRepository);
            }
            var checkRepositoryRole = await _unitOfWork.RepositoryRoles.FindUserRepositoryRole(request.UserId, request.RepositoryId);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (checkRepositoryRole != null)
                {
                    _unitOfWork.RepositoryRoles.DeleteById(checkRepositoryRole.RepositoryRoleId);
                }
                var newRecord = _mapper.Map<RepositoryRole>(request);
                await _unitOfWork.RepositoryRoles.AddAsync(newRecord);
                var affectedRows = await _unitOfWork.CompleteAsync();
                if (affectedRows <= 0)
                    throw new Exception();
                await _unitOfWork.CommitTransactionAsync();
                var result = _mapper.Map<RepositoryRoleCreateCommandResult>(newRecord);
                return Result<RepositoryRoleCreateCommandResult>.Success(result, 201,StringValues.CreateSuccess);
            }
            catch (Exception ex) 
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<RepositoryRoleCreateCommandResult>.Exception(ex.Message);
            }
        }
    }
}
