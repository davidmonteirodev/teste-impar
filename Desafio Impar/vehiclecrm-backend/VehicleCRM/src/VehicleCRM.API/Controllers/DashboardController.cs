using MediatR;
using Microsoft.AspNetCore.Mvc;
using VehicleCRM.Application.Features.Dashboard;
using VehicleCRM.Application.Features.Dashboard.Queries;

namespace VehicleCRM.API.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DashboardResponse))]
        public async Task<IActionResult> Get()
        {
            var query = new GetDashboardQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
