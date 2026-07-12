using MediatR;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Domain.Customers.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Entities;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed class CreateSaleOpportunityCommandHandler : IRequestHandler<CreateSaleOpportunityCommand, EntityCreatedResponse>
    {
        private readonly ISaleOpportunityRepository _saleOpportunityRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public CreateSaleOpportunityCommandHandler(
            ISaleOpportunityRepository saleOpportunityRepository,
            ICustomerRepository customerRepository,
            IVehicleRepository vehicleRepository)
        {
            _saleOpportunityRepository = saleOpportunityRepository;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<EntityCreatedResponse> Handle(CreateSaleOpportunityCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);

            var saleOpportunity = new SaleOpportunity(
                customer.Id,
                vehicle.Id,
                request.Status,
                request.ProposedValue,
                request.Notes);

            await _saleOpportunityRepository.InsertAsync(saleOpportunity, cancellationToken);

            return new EntityCreatedResponse(saleOpportunity.Id, saleOpportunity.CreateDate);
        }
    }
}
