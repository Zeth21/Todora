using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TaskPhoto
    {
        public int PhotoId { get; set; }
        public int TaskId { get; set; }
        public required string PhotoPath { get; set; }
        public DateTime PhotoUploadDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public required string UserId { get; set; }
        public WorkTask Task { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
