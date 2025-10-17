using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class StageNotePhotos
    {
        public int PhotoId { get; set; }
        public int StageNoteId { get; set; }
        public required string PhotoPath { get; set; }

        public virtual StageNote StageNote { get; set; } = null!;
    }
}
