using Application.CQRS.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.CQRS.Commands.WorkTaskCommands
{
    public class WorkTaskUpdateCommand : IRequest<Result<object>>
    {
        [JsonIgnore]
        public string UserId { get; set; } = string.Empty;
        public int WorkTaskId { get; set; }
        public required string WorkTaskName { get; set; }
    }
}
