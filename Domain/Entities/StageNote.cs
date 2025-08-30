namespace Domain.Entities
{
    public class StageNote
    {
        public int StageNoteId { get; set; }
        public int TaskStageId { get; set; }
        public required string UserId { get; set; }
        public DateTime StageNoteDate { get; set; }
        public required string StageNoteText { get; set; }

        public virtual TaskStage TaskStage { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
