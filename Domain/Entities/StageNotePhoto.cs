using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class StageNotePhoto
    {
        public int PhotoId { get; set; }
        public int StageNoteId { get; set; }
        public required string PhotoPath { get; set; }
        public DateTime PhotoUploadDate { get; set; } = DateTime.UtcNow;
        public required string UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public User User { get; set; } = null!;
        public StageNote StageNote { get; set; } = null!;
    }
}
