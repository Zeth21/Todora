using Application.CQRS.Commands.UserCommands;
using Application.CQRS.Results;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Domain.Strings;
using AutoMapper;
using Application.Interfaces.UnitOfWork;

namespace Application.CQRS.Handlers.UserHandlers
{
    public class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, Result<object>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public UserCreateCommandHandler(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<Result<object>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var checkEmail = await _userManager.FindByEmailAsync(request.Email);
            if (checkEmail != null)
            {
                return Result<object>.Fail(StringValues.EmailHasTaken);
            }
            var checkUserName = await _userManager.FindByNameAsync(request.UserName);
            if (checkUserName != null)
            {
                return Result<object>.Fail(StringValues.UserNameHasTaken);
            }
            var newUser = _mapper.Map<User>(request);
            var createResult = await _userManager.CreateAsync(newUser, request.Password);
            var addToRoleResult = await _userManager.AddToRoleAsync(newUser, StringValues.UserRole);
            if (!createResult.Succeeded || !addToRoleResult.Succeeded)
                throw new Exception();
            return Result<object>.Success(data:null,message:StringValues.CreateUserSuccess);
        }
    }
}
