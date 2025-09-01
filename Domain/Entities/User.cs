using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    //This entity keeps User informations

    public class User : IdentityUser
    {
        public string UserPhotoPath { get; set; } = "default-profile.png";
        public bool UserIsActive { get; set; } = true;

        public ICollection<RepositoryRole> RepositoryRoles { get; set; } = new List<RepositoryRole>();
        public ICollection<TaskStage> TaskStages { get; set; } = new List<TaskStage>();
        public ICollection<TaskOwning> TaskOwnings { get; set; } = new List<TaskOwning>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<StageNote> StageNotes { get; set; } = new List<StageNote>();
        public ICollection<WorkTask> Tasks { get; set; } = new List<WorkTask>();
    }
}
