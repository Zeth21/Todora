using Application.CQRS.Commands.RepositoryCommands;
using Application.CQRS.Queries.RepositoryQueries;
using Application.CQRS.Results.RepositoryResults;
using Azure.Core;
using Domain.Strings;
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

        [Authorize(Roles = "User,Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateRepository([FromBody] RepositoryCreateCommand request, CancellationToken cancellationToken = default) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(StringValues.Unauthorized);
            }
            request.UserId = userId;
            var result = await _mediator.Send(request, cancellationToken);
            if (result.IsSucceeded)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.StatusCode, result);
        }

            [Authorize(Roles = "User,Admin")]
            [HttpGet("workings")]
            public async Task<IActionResult> UserGetWorkingRepositories(CancellationToken cancellationToken = default) 
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized(StringValues.Unauthorized);
                }
                var request = new RepositoryGetUserWorkingsQuery { UserId = userId };
                var result = await _mediator.Send(request, cancellationToken);
                if (result.IsSucceeded)
                {
                    return Ok(result.Data);
                }
                return StatusCode(result.StatusCode, result);
            }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("ownings")]
        public async Task<IActionResult> UserGetOwningRepositories(CancellationToken cancellationToken = default)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(StringValues.Unauthorized);
            }
            var request = new RepositoryGetUserOwningsQuery { UserId = userId };
            var result = await _mediator.Send(request, cancellationToken);
            if (result.IsSucceeded)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
