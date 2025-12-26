namespace Domain.Entities
{
    public class StageNote
    {
        public int StageNoteId { get; set; }
        public int TaskStageId { get; set; }
        public required string UserId { get; set; }
        public DateTime StageNoteDate { get; set; }
        public required string StageNoteText { get; set; }

        public TaskStage TaskStage { get; set; } = null!;
        public User User { get; set; } = null!;
        public ICollection<StageNotePhoto> StageNotePhotos { get; set; } = new List<StageNotePhoto>();
    }
}
