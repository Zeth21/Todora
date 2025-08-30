namespace Domain.Entities
{
    //This entity keeps repository(to-do group/working area) informations
    public class Repository
    {
        public int RepositoryId { get; set; }
        public required string RepositoryTitle { get; set; }
        public required string RepositoryDescription { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime RepositoryCreateDate { get; set; } = DateTime.Now;


        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<RepositoryRole> RepositoryRoles { get; set; } = new List<RepositoryRole>();
    }
}
