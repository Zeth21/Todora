namespace Domain.Entities
{
    //This entity stands for keeping the group members authorization roles in repositories
    public class RepositoryRole
    {
        public int RepositoryRoleId { get; set; }
        public int RoleId { get; set; }
        public int RepositoryId { get; set; }
        public required string UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool RepositoryRoleIsDeleted { get; set; } = false;

        public Role Role { get; set; } = null!;
        public Repository Repository { get; set; } = null!;
        public User User { get; set; } = null!; 
    }
}
