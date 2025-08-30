using Domain.Enum;

namespace Domain.Entities
{
    //This entity represents task informations
    public class Task
    {
        public int TaskId { get; set; }
        public int RepositoryId { get; set; }
        public required string TaskTitle { get; set; }
        public required string TaskDescription { get; set; }
        public DateTime TaskStartDate { get; set; } = DateTime.Now;
        public DateTime? TaskEndDate { get; set; }
        public required TaskRole TaskRole { get; set; } = 0;


        public virtual Repository Repository { get; set; } = null!;
        public ICollection<TaskStage> TaskStages { get; set; } = new List<TaskStage>();
    }
}
