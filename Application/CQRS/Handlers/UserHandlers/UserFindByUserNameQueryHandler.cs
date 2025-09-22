using Application.CQRS.Queries.UserQueries;
using Application.CQRS.Results;
using Application.CQRS.Results.UserResults;
using AutoMapper;
using Domain.Entities;
using Domain.Strings;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Handlers.UserHandlers
{
    public class UserFindByUserNameQueryHandler : IRequestHandler<UserFindByUserNameQuery, Result<List<UserFindByUserNameQueryResult>>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public UserFindByUserNameQueryHandler(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public Task<Result<List<UserFindByUserNameQueryResult>>> Handle(UserFindByUserNameQuery request, CancellationToken cancellationToken)
        {
            var users = _userManager.
                Users.Where(x => x.UserName!.StartsWith(request.UserName))
                .ToList();
            if (users.Count <= 0) 
            {
                return Task.FromResult(Result<List<UserFindByUserNameQueryResult>>.NoContent(StringValues.AllHasFoundSuccessfully));
            }
            var result = _mapper.Map<List<UserFindByUserNameQueryResult>>(users);
            return Task.FromResult(Result<List<UserFindByUserNameQueryResult>>.Success(data: result, message: StringValues.AllHasFoundSuccessfully));
        }
    }
}
