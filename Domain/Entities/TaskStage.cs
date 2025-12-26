namespace Domain.Entities
{
    //This entity represents task-stage relationship informations
    public class TaskStage
    {
        public int TaskStageId { get; set; }
        public int TaskId { get; set; }
        public int StageId { get; set; }
        public required string UserId { get; set; }
        public DateTime TaskStageDate { get; set; } = DateTime.UtcNow;

        public WorkTask Task { get; set; } = null!;
        public Stage Stage { get; set; } = null!;
        public User User { get; set; } = null!;
        public ICollection<StageNote> StageNotes { get; set; } = new List<StageNote>();
    }
}
