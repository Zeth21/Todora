using Application.CQRS.Commands.RepositoryRoleCommands;
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
    public class RepositoryRoleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RepositoryRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = ("Admin,User"))]
        [HttpPost]
        public async Task<IActionResult> CreateRepositoryRole([FromBody] RepositoryRoleCreateCommand request, CancellationToken cancellationToken = default)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(StringValues.Unauthorized);
            }
            request.AuthorizedPersonId = userId;
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRepositoryRole(int id, [FromBody] RoleValues roleValue, CancellationToken cancellationToken = default) 
        {
            var request = new RepositoryRoleUpdateCommand
            {
                RepositoryRoleId = id,
                RoleValue = roleValue
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
