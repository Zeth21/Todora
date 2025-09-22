using Application.CQRS.Queries.UserQueries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Validators.QueryValidators.UserQueryValidators
{
    public class UserFindByUserNameQueryValidator : AbstractValidator<UserFindByUserNameQuery>
    {
        public UserFindByUserNameQueryValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Name cannot be empty.")
                .Matches("^[a-zA-Z0-9 -]+$").WithMessage("Username can only contain letters, numbers, and hyphens.")
                .MaximumLength(30).WithMessage("Username cannot exceed 30 characters.");
        }
    }
}
