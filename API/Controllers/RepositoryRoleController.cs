using Application.CQRS.Commands.RepositoryRoleCommands;
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
    public class RepositoryRoleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RepositoryRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles =("Admin,User"))]
        [HttpPost("create")]
        public async Task<IActionResult> CreateRepositoryRole([FromBody] RepositoryRoleCreateCommand request, CancellationToken cancellationToken = default) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(StringValues.Unauthorized);
            }
            request.AuthorizedPersonId = userId;
            var result = await _mediator.Send(request, cancellationToken);
            if (result.IsSucceeded)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
