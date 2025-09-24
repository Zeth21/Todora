using Application.CQRS.Commands.RepositoryCommands;
using Domain.Values;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Validators.CommandValidators.RepositoryCommandValidators
{
    public class RepositoryCreateCommandValidator : AbstractValidator<RepositoryCreateCommand>
    {
        public RepositoryCreateCommandValidator()
        {
            RuleFor(x => x.RepositoryTitle)
                .NotEmpty().WithMessage(StringValues.NameCannotBeNull)
                .MaximumLength(75).WithMessage(StringValues.RepositoryCreateFailTitleSame);

            RuleFor(x => x.RepositoryDescription)
                .NotEmpty().WithMessage(StringValues.DescriptionCannotBeNull)
                .MaximumLength(75).WithMessage(StringValues.RepositoryCreateFailDescriptionLength);
        }
    }
}
