using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    //This entity stores roles, we need it for database normalization
    public class Role 
    {
        public int RoleId { get; set; }
        public required string RoleName { get; set; }

        public ICollection<RepositoryRole> RepositoryRoles { get; set; } = new List<RepositoryRole>();
    }
}
