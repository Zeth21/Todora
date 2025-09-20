using Application.CQRS.Commands.UserCommands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Validators.CommandValidators.UserCommandValidators
{
    public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
    {
        public UserCreateCommandValidator()
        {

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Name cannot be empty.")
                .Matches("^[a-zA-Z0-9 -]+$").WithMessage("Username can only contain letters, numbers, and hyphens.")
                .MaximumLength(30).WithMessage("Username cannot exceed 30 characters.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name cannot be empty.")
                .Matches("^[a-zA-Z0-9 -]+$").WithMessage("Name can only contain letters, numbers, and hyphens.")
                .MaximumLength(30).WithMessage("Name cannot exceed 30 characters.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Name cannot be empty.")
                .Matches("^[a-zA-Z0-9 -]+$").WithMessage("Surname can only contain letters, numbers, and hyphens.")
                .MaximumLength(30).WithMessage("Surname cannot exceed 30 characters.");

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
