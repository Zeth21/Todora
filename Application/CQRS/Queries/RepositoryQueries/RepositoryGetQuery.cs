using Application.CQRS.Results;
using Application.CQRS.Results.RepositoryResults;
using Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Queries.RepositoryQueries
{
    public class RepositoryGetQuery : IRequest<Result<RepositoryGetResult>>
    {
        public string UserId { get; set; } = string.Empty;
        public RepositoryRelationType Type { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
