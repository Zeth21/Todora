using Application.CQRS.Queries.RoleQueries;
using Application.CQRS.Results;
using Application.CQRS.Results.RoleResults;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Values;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Handlers.RoleHandlers
{
    public class RoleGetAllQueryHandler : IRequestHandler<RoleGetAllQuery, Result<List<RoleGetAllQueryResult>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<RoleGetAllQueryResult>>> Handle(RoleGetAllQuery request, CancellationToken cancellationToken)
        {
            var roles = await _unitOfWork.Roles.GetAllRoles();
            if (roles is null) 
            {
                return Result<List<RoleGetAllQueryResult>>.NotFound(StringValues.NothingHasFound);
            }
            var result = _mapper.Map<List<RoleGetAllQueryResult>>(roles);
            return Result<List<RoleGetAllQueryResult>>.Success(data:result,message:StringValues.AllHasFoundSuccessfully);
        }
    }
}
