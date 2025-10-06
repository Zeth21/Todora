using Application.CQRS.Results;
using Application.CQRS.Results.WorkTaskResults;
using Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.CQRS.Commands.WorkTaskCommands
{
    public class WorkTaskCreateCommand : IRequest<Result<WorkTaskCreateCommandResult>>
    {
        public int RepositoryId { get; set; }
        public required string TaskTitle { get; set; }
        public required string TaskDescription { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public TaskRole TaskRole { get; set; }

        [JsonIgnore]
        public string TaskCreatedUserId { get; set; } = string.Empty;
    }
}
