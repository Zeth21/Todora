using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Behaviors.ValidationBehaviors
{
    using FluentValidation;
    using MediatR;

    namespace Application.Common.Behaviors
    {
        public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
        {
            private readonly IEnumerable<IValidator<TRequest>> _validators;

            public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
            {
                _validators = validators;
            }

            public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                // If there is no validatior for dto move on.
                if (!_validators.Any())
                {
                    return await next();
                }

                var context = new ValidationContext<TRequest>(request);

                // Run all validators for dto and get all errors.
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                // If there is one or more validation error throw a validationException. 
                // We will catch that exception on GlobalExceptionHandlingMiddleware.
                if (failures.Any())
                {
                    throw new ValidationException(failures);
                }

                // If there is no error go to handler.
                return await next();
            }
        }
    }
}
