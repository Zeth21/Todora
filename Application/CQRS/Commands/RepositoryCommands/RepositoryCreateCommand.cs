using Application.CQRS.Results;
using Application.CQRS.Results.RepositoryResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.CQRS.Commands.RepositoryCommands
{
    public class RepositoryCreateCommand : IRequest<Result<RepositoryCreateCommandResult>>
    {
        [JsonIgnore]
        public string UserId { get; set; } = string.Empty;
        public IFormFile? RepositoryPhoto { get; set; }
        public string RepositoryTitle { get; set; } = null!;
        public string RepositoryDescription { get; set; } = null!;

    }
}
