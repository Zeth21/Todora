using Application.Authorization;
using Application.Interfaces.Data.Security;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization.Infrastructure;


namespace Application.Services.Security
{
    public class AuthorizationRuleService : IAuthorizationRules
    {
        public bool IsAuthorized(RoleValues userRole, ProjectOperationRequirement requirement, object resource)
        {
            // Returning correct method for incoming resource
            // Resource is an entity, that user wants to do CRUD operations on it.
            return resource switch
            {
                WorkTask => CheckWorkTaskRules(userRole, requirement),
                RepositoryRole => CheckRepositoryRoleRules(userRole, requirement),
                TaskOwning => CheckTaskOwningRules(userRole, requirement),
                Repository => CheckRepositoryRules(userRole, requirement),
                TaskStage => CheckTaskStageRules(userRole, requirement),
                _ => false 
            };
        }

        //These methods checks if that users role allowed to do that requirement
        private bool CheckWorkTaskRules(RoleValues userRole, ProjectOperationRequirement requirement)
        {
            if (requirement == Operations.Read)
                return true;

            if (requirement == Operations.Delete)
                return userRole == RoleValues.Manager || userRole == RoleValues.Owner;

            if (requirement == Operations.Create)
                return userRole == RoleValues.Manager || userRole == RoleValues.Owner;

            if(requirement == Operations.Update)
                return userRole == RoleValues.Manager || userRole == RoleValues.Owner;

            return false;
        }


        private bool CheckRepositoryRoleRules(RoleValues userRole, ProjectOperationRequirement requirement)
        {
            if (requirement == Operations.Create || requirement == Operations.Update || requirement == Operations.Delete)
                return userRole == RoleValues.Owner || userRole == RoleValues.Manager;

            if (requirement == Operations.Read)
                return userRole == RoleValues.Manager || userRole == RoleValues.Owner || userRole == RoleValues.ManagerGuest;

            return false;
        }

        private bool CheckTaskOwningRules(RoleValues userRole, ProjectOperationRequirement requirement) 
        {
            if (requirement == Operations.Create || requirement == Operations.Delete || requirement == Operations.Update)
                return userRole == RoleValues.Owner || userRole == RoleValues.Manager;

            if (requirement == Operations.Read)
                return true;
            
            return false;
        }

        private bool CheckRepositoryRules(RoleValues userRole, ProjectOperationRequirement requirement) 
        {
            if (requirement == Operations.Create || requirement == Operations.Delete || requirement == Operations.Update)
                return userRole == RoleValues.Owner || userRole == RoleValues.Manager;

            if (requirement == Operations.Read)
                return true;

            return false;
        }

        private bool CheckTaskStageRules(RoleValues userRole, ProjectOperationRequirement requirement) 
        {
            if (requirement == Operations.Create || requirement == Operations.Delete || requirement == Operations.Update)
                return userRole == RoleValues.Owner || userRole == RoleValues.Manager;

            if (requirement == Operations.Read)
                return true;

            return false;
        }
    }
}