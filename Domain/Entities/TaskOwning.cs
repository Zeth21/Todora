namespace Domain.Entities
{
    //This entity keeps task-user owning informations
    public class TaskOwning
    {
        public int TaskOwningId { get; set; }
        public int TaskId { get; set; }
        public required string UserId {  get; set; }
        public DateTime TaskOwningDate { get; set; } = DateTime.UtcNow;
        public DateTime? TaskOwningEndDate { get; set; }
        public bool TaskOwningIsActive { get; set; } = true;
        public WorkTask Task { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
