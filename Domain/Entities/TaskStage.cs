namespace Domain.Entities
{
    //This entity represents task-stage relationship informations
    public class TaskStage
    {
        public int TaskStageId { get; set; }
        public int TaskId { get; set; }
        public int StageId { get; set; }
        public required string UserId { get; set; }
        public string? TaskStageNote { get; set; }
        public DateTime TaskStageDate { get; set; } = DateTime.Now;

        public virtual Task Task { get; set; } = null!;
        public virtual Stage Stage { get; set; } = null!;
        public virtual User User { get; set; } = null!;

    }
}
