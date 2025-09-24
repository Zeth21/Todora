using Application.CQRS.Queries.UserQueries;
using Application.CQRS.Results;
using Application.CQRS.Results.UserResults;
using Application.Interfaces.Data.Security;
using AutoMapper;
using Domain.Entities;
using Domain.Values;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.Handlers.UserHandlers
{
    public class UserLoginQueryHandler : IRequestHandler<UserLoginQuery, Result<UserLoginQueryResult>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJWTGenerator _jwtGenerator;

        public UserLoginQueryHandler(UserManager<User> userManager, IJWTGenerator jwtGenerator)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
        }
        public async Task<Result<UserLoginQueryResult>> Handle(UserLoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) 
            {
                return Result<UserLoginQueryResult>.Fail(StringValues.LoginFail);
            }
            bool checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!checkPassword) 
            {
                return Result<UserLoginQueryResult>.Fail(StringValues.LoginFail);
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var token =  _jwtGenerator.GenerateToken(user, userRoles);
            var result = new UserLoginQueryResult 
            {
                Token = token
            };
            return Result<UserLoginQueryResult>.Success(message: StringValues.LoginSuccess,data: result);
        }
    }
}
