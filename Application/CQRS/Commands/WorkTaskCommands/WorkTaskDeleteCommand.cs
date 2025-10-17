using Application.CQRS.Results;
using Application.CQRS.Results.WorkTaskResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.CQRS.Commands.WorkTaskCommands
{
    public class WorkTaskDeleteCommand : IRequest<Result<WorkTaskDeleteCommandResult>>
    {
        [JsonIgnore]
        public int WorkTaskId { get; set; }
    }
}
