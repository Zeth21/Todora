using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Results.WorkTaskResults
{
    public class WorkTaskCreateCommandResult
    {
        public int TaskId { get; set; }
        public int RepositoryId { get; set; }
        public required string TaskTitle { get; set; }
        public required string TaskDescription { get; set; }
        public required string TaskCreatedUserId { get; set; }
        public required string TaskCreatedUserName { get; set; }
        public DateTime TaskCreateDate { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public TaskRole TaskRole { get; set; }
        public bool TaskIsCompleted { get; set; } = false;
        public bool TaskIsRemoved { get; set; } = false;
    }
}
