using Domain.Enum;

namespace Domain.Entities
{
    //This entity represents task informations
    public class WorkTask
    {
        public int TaskId { get; set; }
        public int RepositoryId { get; set; }
        public required string TaskTitle { get; set; }
        public required string TaskDescription { get; set; }
        public required string TaskCreatedUserId { get; set; }
        public DateTime TaskCreateDate { get; set; } = DateTime.UtcNow;
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public int? TaskRoleId { get; set; }
        public bool TaskIsCompleted { get; set; } = false;
        public bool TaskIsRemoved { get; set; } = false;
        public int? TaskCurrentStageId { get; set; }


        public User User { get; set; } = null!;
        public Role? Role { get; set; }
        public TaskStage? CurrentTaskStage { get; set; }
        public Repository Repository { get; set; } = null!;
        public ICollection<TaskStage> TaskStages { get; set; } = new List<TaskStage>();
        public ICollection<TaskOwning> TaskOwnings { get; set; } = new List<TaskOwning>();
        public ICollection<TaskPhoto> TaskPhotos { get; set; } = new List<TaskPhoto>();
    }
}
