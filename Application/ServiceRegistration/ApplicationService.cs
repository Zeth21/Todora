using Application.CQRS.Behaviors.ValidationBehaviors.Application.Common.Behaviors;
using Application.MappingProfiles;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            return services;
        }
    }
}