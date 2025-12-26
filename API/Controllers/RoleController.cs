using Application.CQRS.Queries.RoleQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken = default) 
        {
            var request = new RoleGetAllQuery();
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode(result.StatusCode,result);
        }
    }
}
