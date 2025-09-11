using Application.CQRS.Queries.UserQueries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Validators.QueryValidators
{
    public class UserLoginQueryValidator : AbstractValidator<UserLoginQuery>
    {
        public UserLoginQueryValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The email address cannot be empty.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The password cannot be empty.")
                .MinimumLength(8).WithMessage("The password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("The password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("The password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("The password must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("The password must contain at least one special character.");
        }
    }
}
