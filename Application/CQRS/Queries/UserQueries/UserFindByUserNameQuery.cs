using Application.CQRS.Results;
using Application.CQRS.Results.UserResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Queries.UserQueries
{
    public class UserFindByUserNameQuery : IRequest<Result<List<UserFindByUserNameQueryResult>>>
    {
        public string UserName { get; set; } = null!; 
    }
}
