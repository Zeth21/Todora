using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Results.RepositoryResults
{
    public class RepositoryCreateCommandResult
    {
        public int RepositoryId { get; set; }
        public string RepositoryPhotoPath { get; set; } = null!;
        public string RepositoryTitle { get; set; } = null!;
        public string RepositoryDescription { get; set; } = null!;
        public DateTime RepositoryCreateDate { get; set; }
        public List<RepositoryMember> RepositoryMembers { get; set; } = new List<RepositoryMember>();
        public List<RepositoryTask> RepositoryTasks { get; set; } = new List<RepositoryTask>();
    }
    public class RepositoryMember 
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string RoleName { get; set; } = null!;
    }
    public class RepositoryTask 
    {
        public int TaskId { get; set; }
        public string TaskTitle { get; set; } = null!;
        public bool TaskIsCompleted { get; set; }

    }
}
