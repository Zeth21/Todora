namespace Domain.Entities
{
    //This entity stores stage informations, we need it for database normalization
    public class Stage
    {
        public int StageId { get; set; }
        public required string StageName { get; set; }

        public ICollection<TaskStage> TaskStages { get; set; } = new List<TaskStage>();
    }
}
