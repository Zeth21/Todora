using Application.CQRS.Commands.RepositoryCommands;
using Application.CQRS.Results;
using Application.CQRS.Results.RepositoryResults;
using Application.Exceptions;
using Application.Interfaces.File;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using Domain.Values;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.CQRS.Handlers.RepositoryHandlers
{
    public class RepositoryCreateCommandHandler : IRequestHandler<RepositoryCreateCommand, Result<RepositoryCreateCommandResult>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        public RepositoryCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
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
            if (request.RepositoryPhoto is not null)
            {
                var photos = new List<IFormFile>() { request.RepositoryPhoto };
                var filePaths = await _fileService.SavePhotosAsync(photos, StringValues.RepositoryPhotoFolder);
                newRepository.PhotoPath = filePaths.FirstOrDefault()!;
            }
            await _unitOfWork.Repositories.AddAsync(newRepository);
            await _unitOfWork.CompleteAsync();
            var result = _mapper.Map<RepositoryCreateCommandResult>(newRepository);
            return Result<RepositoryCreateCommandResult>.Success(data: result, message: StringValues.RepositoryCreateSuccess);
        }
    }
}
