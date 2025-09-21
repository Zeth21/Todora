using Application.CQRS.Results;
using Application.CQRS.Results.RepositoryResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Queries.RepositoryQueries
{
    public class RepositoryGetUserOwningsQuery : IRequest<Result<List<RepositoryGetUserOwningsQueryResult>>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
