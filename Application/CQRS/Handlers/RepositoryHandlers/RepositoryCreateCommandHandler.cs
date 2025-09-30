using Application.CQRS.Commands.RepositoryCommands;
using Application.CQRS.Results;
using Application.CQRS.Results.RepositoryResults;
using Application.Exceptions;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using Domain.Values;
using MediatR;

namespace Application.CQRS.Handlers.RepositoryHandlers
{
    public class RepositoryCreateCommandHandler : IRequestHandler<RepositoryCreateCommand, Result<RepositoryCreateCommandResult>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RepositoryCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<RepositoryCreateCommandResult>> Handle(RepositoryCreateCommand request, CancellationToken cancellationToken)
        {
            var checkTitle = await _unitOfWork.Repositories.GetUserRepositoryByTitleAsync(request.UserId, request.RepositoryTitle);
            if (checkTitle != null)
                return Result<RepositoryCreateCommandResult>.Fail(StringValues.RepositoryCreateFailTitleSame);
            var newRepository = _mapper.Map<Repository>(request);
            newRepository.RepositoryRoles = new List<RepositoryRole>
            {
                new RepositoryRole { UserId = request.UserId, RoleId = (int)RoleValues.Owner }
            };
            await _unitOfWork.Repositories.AddAsync(newRepository);

            var affectedRows = await _unitOfWork.CompleteAsync();
            if (affectedRows < 2)
                throw new SaveDataException(StringValues.SaveFail, new Exception());
            var result = _mapper.Map<RepositoryCreateCommandResult>(newRepository);
            return Result<RepositoryCreateCommandResult>.Success(data: result, message: StringValues.RepositoryCreateSuccess);
        }
    }
}
