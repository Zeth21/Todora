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
    public class RepositoryGetUserOwningsQueryHandler : IRequestHandler<RepositoryGetUserOwningsQuery, Result<List<RepositoryGetUserOwningsQueryResult>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RepositoryGetUserOwningsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<List<RepositoryGetUserOwningsQueryResult>>> Handle(RepositoryGetUserOwningsQuery request, CancellationToken cancellationToken)
        {
            var repositories = await _unitOfWork.Repositories.GetUserOwningRepositoriesByUserId(request.UserId);
            var result = _mapper.Map<List<RepositoryGetUserOwningsQueryResult>>(repositories);
            return Result<List<RepositoryGetUserOwningsQueryResult>>.Success(data:result, message:StringValues.AllHasFoundSuccessfully);
        }
    }
}
