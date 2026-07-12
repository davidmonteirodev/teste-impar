using MediatR;
using Microsoft.AspNetCore.Mvc;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Application.Features.Customers;
using VehicleCRM.Application.Features.Customers.Commands;
using VehicleCRM.Application.Features.Customers.Queries;

namespace VehicleCRM.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves all customers with filtering and pagination
        /// </summary>
        /// <param name="query">Query parameters for filtering and pagination</param>
        /// <returns>A paginated collection of customers</returns>
        /// <response code="200">Successfully retrieved customers</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<CustomerResponse>))]
        public async Task<IActionResult> GetAll([FromQuery] GetCustomersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific customer by ID
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <returns>The requested customer</returns>
        /// <response code="200">Successfully retrieved customer</response>
        /// <response code="404">Customer not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetCustomerByIdQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="request">Customer creation details</param>
        /// <returns>The created customer with ID and creation date</returns>
        /// <response code="201">Customer successfully created</response>
        /// <response code="400">Invalid customer data</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EntityCreatedResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCustomerCommand request)
        {
            var result = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="request">Updated customer details</param>
        /// <response code="204">Customer successfully updated</response>
        /// <response code="400">Invalid customer data</response>
        /// <response code="404">Customer not found</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerCommand request)
        {
            await _mediator.Send(request);
            return NoContent();
        }

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <response code="204">Customer successfully deleted</response>
        /// <response code="404">Customer not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var command = new DeleteCustomerCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
