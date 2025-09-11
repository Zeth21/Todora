using Application.CQRS.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginQuery request, CancellationToken cancellationToken = default) 
        {
            var result = await _mediator.Send(request, cancellationToken);
            if(result.IsSucceeded)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.StatusCode,result);
        }
    }
}
