using Application.CQRS.Behaviors.ValidationBehaviors.Application.Common.Behaviors;
using Application.Interfaces.Data.Security;
using Application.MappingProfiles;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationRules, AuthorizationRuleService>();
            services.AddGenericAuthorizationHandlers();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            return services;
        }

        private static IServiceCollection AddGenericAuthorizationHandlers(this IServiceCollection services)
        {
            var authorizableTypes = new[]
            {
                typeof(WorkTask),
                typeof(Stage),
                typeof(RepositoryRole),
                typeof(TaskOwning),
                typeof(TaskStage),
                typeof(Repository),
                typeof(StageNote)
            };

            foreach (var type in authorizableTypes)
            {
                var genericHandlerType = typeof(ResourceAuthorization<>);
                var specificHandlerType = genericHandlerType.MakeGenericType(type);

                services.AddScoped(typeof(IAuthorizationHandler), specificHandlerType);
            }

            return services;
        }
    }
}