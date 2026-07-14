using MediatR;
using Microsoft.AspNetCore.Mvc;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Application.Features.SaleOpportunities;
using VehicleCRM.Application.Features.SaleOpportunities.Commands;
using VehicleCRM.Application.Features.SaleOpportunities.Queries;

namespace VehicleCRM.API.Controllers
{
    [ApiController]
    [Route("api/sale-opportunities")]
    public class SaleOpportunitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SaleOpportunitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves all sale opportunities with filtering and pagination
        /// </summary>
        /// <param name="query">Query parameters for filtering and pagination</param>
        /// <returns>A paginated collection of sale opportunities</returns>
        /// <response code="200">Successfully retrieved sale opportunities</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<SaleOpportunityResponse>))]
        public async Task<IActionResult> GetAll([FromQuery] GetSaleOpportunitiesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific sale opportunity by ID
        /// </summary>
        /// <param name="id">The sale opportunity ID</param>
        /// <returns>The requested sale opportunity</returns>
        /// <response code="200">Successfully retrieved sale opportunity</response>
        /// <response code="404">Sale opportunity not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaleOpportunityResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetSaleOpportunityByIdQuery(id));
            return result is null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Creates a new sale opportunity
        /// </summary>
        /// <param name="request">Sale opportunity creation details</param>
        /// <returns>The created sale opportunity with ID and creation date</returns>
        /// <response code="201">Sale opportunity successfully created</response>
        /// <response code="400">Invalid sale opportunity data</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EntityCreatedResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSaleOpportunityCommand request)
        {
            var result = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an existing sale opportunity
        /// </summary>
        /// <param name="request">Updated sale opportunity details</param>
        /// <response code="204">Sale opportunity successfully updated</response>
        /// <response code="400">Invalid sale opportunity data</response>
        /// <response code="404">Sale opportunity not found</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateSaleOpportunityCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes a sale opportunity
        /// </summary>
        /// <param name="id">The sale opportunity ID</param>
        /// <response code="204">Sale opportunity successfully deleted</response>
        /// <response code="404">Sale opportunity not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var command = new DeleteSaleOpportunityCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}