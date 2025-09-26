using Application.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Interfaces.Data.Security
{
    public class ResourceAuthorization<TResource> : AuthorizationHandler<ProjectOperationRequirement, TResource>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationRules _ruleService;
        private readonly IRepositoryContextResolver _contextResolver;

        public ResourceAuthorization(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IAuthorizationRules ruleService,
            IRepositoryContextResolver contextResolver)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _ruleService = ruleService;
            _contextResolver = contextResolver;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ProjectOperationRequirement requirement,
            TResource resource)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            var repositoryId = await _contextResolver.GetRepositoryIdForResourceAsync(resource);

            if (repositoryId == null)
            {
                return;
            }

            var userRoleInRepo = await _unitOfWork.RepositoryRoles.FindUserRepositoryRole(userId, repositoryId.Value);

            if (userRoleInRepo == null)
            {
                return;
            }

            var userRole = userRoleInRepo.Role.RoleName;

            if (_ruleService.IsAuthorized(userRole, requirement, resource))
            {
                context.Succeed(requirement);
            }

        }
    }
}