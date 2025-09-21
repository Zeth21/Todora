using Application.CQRS.Queries.RepositoryQueries;
using Application.CQRS.Results;
using Application.CQRS.Results.RepositoryResults;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Handlers.RepositoryHandlers
{
    public class RepositoryGetUserWorkingsQueryHandler : IRequestHandler<RepositoryGetUserWorkingsQuery, Result<List<RepositoryGetUserWorkingsQueryResult>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RepositoryGetUserWorkingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<List<RepositoryGetUserWorkingsQueryResult>>> Handle(RepositoryGetUserWorkingsQuery request, CancellationToken cancellationToken)
        {
            var repositories = await _unitOfWork.Repositories.GetUserWorkingRepositoriesByUserId(request.UserId);
            var result = _mapper.Map<List<RepositoryGetUserWorkingsQueryResult>>(repositories);
            return Result<List<RepositoryGetUserWorkingsQueryResult>>.Success(data:result);
        }
    }
}
