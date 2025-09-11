using Application.CQRS.Results;
using Application.CQRS.Results.UserResults;
using MediatR;

namespace Application.CQRS.Queries.UserQueries
{
    public class UserLoginQuery : IRequest<Result<UserLoginQueryResult>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
