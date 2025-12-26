using Application.CQRS.Queries.RepositoryQueries;
using Application.CQRS.Results;
using Application.CQRS.Results.RepositoryResults;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Values;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Handlers.RepositoryHandlers
{
    public class RepositoryGetQueryHandler : IRequestHandler<RepositoryGetQuery, Result<RepositoryGetResult>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RepositoryGetQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<RepositoryGetResult>> Handle(RepositoryGetQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Repositories.GetRepositories(request.UserId, request.Type, request.PageSize, request.PageNumber);
            var repositories = _mapper.Map<List<RepositoryItem>>(result.repositories);
            var response = new RepositoryGetResult
            {
                Repositories = repositories,
                TotalRecordCount = result.totalCount,
                TotalPageCount = (int)Math.Ceiling((double)result.totalCount / request.PageSize)
            };
            return Result<RepositoryGetResult>.Success(data:response, message:StringValues.AllHasFoundSuccessfully);
        }
    }
}
