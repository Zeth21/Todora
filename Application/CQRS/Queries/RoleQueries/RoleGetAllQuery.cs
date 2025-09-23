using Application.CQRS.Results;
using Application.CQRS.Results.RoleResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Queries.RoleQueries
{
    public class RoleGetAllQuery : IRequest<Result<List<RoleGetAllQueryResult>>>
    {
    }
}
