using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LogEntry
    {
        public int LogEntryId { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public string? UserId { get; set; }
        public string? TraceId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
