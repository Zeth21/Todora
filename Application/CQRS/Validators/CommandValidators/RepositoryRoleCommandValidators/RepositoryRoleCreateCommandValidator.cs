using Application.CQRS.Commands.RepositoryRoleCommands;
using Domain.Strings;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Validators.CommandValidators.RepositoryRoleCommandValidators
{
    public class RepositoryRoleCreateCommandValidator : AbstractValidator<RepositoryRoleCreateCommand>
    {
        public RepositoryRoleCreateCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(StringValues.InvalidUser);

            RuleFor(x => x.RoleName)
                .IsInEnum()
                .WithMessage(StringValues.InvalidRoleEnum);
        }
    }
}
