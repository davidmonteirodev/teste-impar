using MediatR;
using Microsoft.AspNetCore.Mvc;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Application.Features.Vehicles;
using VehicleCRM.Application.Features.Vehicles.Commands;
using VehicleCRM.Application.Features.Vehicles.Queries;

namespace VehicleCRM.API.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VehiclesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves all vehicles with filtering and pagination
        /// </summary>
        /// <param name="query">Query parameters for filtering and pagination</param>
        /// <returns>A paginated collection of vehicles</returns>
        /// <response code="200">Successfully retrieved vehicles</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<VehicleResponse>))]
        public async Task<IActionResult> GetAll([FromQuery] GetVehiclesQuery query)
            {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific vehicle by ID
        /// </summary>
        /// <param name="id">The vehicle ID</param>
        /// <returns>The requested vehicle</returns>
        /// <response code="200">Successfully retrieved vehicle</response>
        /// <response code="404">Vehicle not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VehicleResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetVehicleByIdQuery(id));
            return result is null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Creates a new vehicle
        /// </summary>
        /// <param name="request">Vehicle creation details</param>
        /// <returns>The created vehicle with ID and creation date</returns>
        /// <response code="201">Vehicle successfully created</response>
        /// <response code="400">Invalid vehicle data</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EntityCreatedResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateVehicleCommand request)
        {
            var result = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an existing vehicle
        /// </summary>
        /// <param name="request">Updated vehicle details</param>
        /// <response code="204">Vehicle successfully updated</response>
        /// <response code="400">Invalid vehicle data or vehicle is reserved/sold</response>
        /// <response code="404">Vehicle not found</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateVehicleCommand request)
        {
            await _mediator.Send(request);
            return NoContent();
        }

        /// <summary>
        /// Deletes a vehicle
        /// </summary>
        /// <param name="id">The vehicle ID</param>
        /// <response code="204">Vehicle successfully deleted</response>
        /// <response code="400">Vehicle has associated sale opportunities</response>
        /// <response code="404">Vehicle not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var command = new DeleteVehicleCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
