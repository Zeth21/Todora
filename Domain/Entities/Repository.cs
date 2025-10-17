namespace Domain.Entities
{
    //This entity keeps repository(to-do group/working area) informations
    public class Repository
    {
        public int RepositoryId { get; set; }
        public string PhotoPath { get; set; } = "default-repo.png";
        public required string RepositoryTitle { get; set; }
        public required string RepositoryDescription { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime RepositoryCreateDate { get; set; } = DateTime.UtcNow;
        public required string RepositoryUserId { get; set; } 

        public ICollection<WorkTask> Tasks { get; set; } = new List<WorkTask>();
        public ICollection<RepositoryRole> RepositoryRoles { get; set; } = new List<RepositoryRole>();
    }
}
