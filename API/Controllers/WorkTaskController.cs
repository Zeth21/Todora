using Application.CQRS.Commands.WorkTaskCommands;
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
    public class WorkTaskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkTaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public async Task<IActionResult> CreateWorkTask([FromBody] WorkTaskCreateCommand request, CancellationToken cancellationToken = default) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(StringValues.Unauthorized);
            }
            request.TaskCreatedUserId = userId;
            var result = await _mediator.Send(request, cancellationToken);
            if (result.IsSucceeded)
            {
                return StatusCode(result.StatusCode,result.Data);
            }
            return StatusCode(result.StatusCode,result);
        }
    }
}
