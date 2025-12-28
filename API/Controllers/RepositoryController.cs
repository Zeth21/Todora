using Application.CQRS.Commands.RepositoryCommands;
using Application.CQRS.Queries.RepositoryQueries;
using Application.CQRS.Results.RepositoryResults;
using Azure.Core;
using Domain.Enum;
using Domain.Values;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RepositoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRepository([FromForm] RepositoryCreateCommand request, CancellationToken cancellationToken = default) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (userId == null)
            //{
            //    return Unauthorized(StringValues.Unauthorized);
            //}
            //request.UserId = userId;
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetRepositories(
            [FromQuery] RepositoryRelationType type,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1,
            CancellationToken cancellationToken = default)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(StringValues.Unauthorized);
            }
            var request = new RepositoryGetQuery { UserId = userId, Type = type, PageSize = pageSize, PageNumber = pageNumber };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
