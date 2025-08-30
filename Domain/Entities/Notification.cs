using Domain.Enum;

namespace Domain.Entities
{
    //This entity keeps user notification informations
    public class Notification
    {
        public int NotificationId { get; set; }
        public required string UserId { get; set; }
        public required string NotificationTitle { get; set; }
        public required string NotificationDescription { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime NotificationTime { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
