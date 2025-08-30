namespace Domain.Entities
{
    //This entity keeps task-user owning informations
    public class TaskOwning
    {
        public int TaskOwningId { get; set; }
        public int TaskId { get; set; }
        public required string UserId {  get; set; }
        public DateTime TaskOwningDate { get; set; } = DateTime.Now;

        public virtual Task Task { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
