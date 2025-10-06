using Application.CQRS.Commands.WorkTaskCommands;
using Domain.Values;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Validators.CommandValidators.WorkTaskCommandValidators
{
    public class WorkTaskCreateCommandValidator : AbstractValidator<WorkTaskCreateCommand>
    {
        public WorkTaskCreateCommandValidator()
        {
            RuleFor(x => x.TaskTitle)
                .NotEmpty().WithMessage(StringValues.NameCannotBeNull)
                .MaximumLength(75).WithMessage(StringValues.InvalidTitleLength75);

            RuleFor(x => x.TaskDescription)
                .NotEmpty().WithMessage(StringValues.DescriptionCannotBeNull)
                .MaximumLength(75).WithMessage(StringValues.InvalidDescriptionLength75);
        }
    }
}
