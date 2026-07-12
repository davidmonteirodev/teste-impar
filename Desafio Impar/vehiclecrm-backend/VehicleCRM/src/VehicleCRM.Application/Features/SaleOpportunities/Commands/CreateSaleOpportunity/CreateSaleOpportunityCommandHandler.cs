using MediatR;
using VehicleCRM.Application.Common.Exceptions;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Domain.Customers.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Entities;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Services;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed class CreateSaleOpportunityCommandHandler : IRequestHandler<CreateSaleOpportunityCommand, EntityCreatedResponse>
    {
        private readonly ISaleOpportunityRepository _saleOpportunityRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ISaleOpportunityDomainService _saleOpportunityDomainService;

        public CreateSaleOpportunityCommandHandler(
            ISaleOpportunityRepository saleOpportunityRepository,
            ICustomerRepository customerRepository,
            IVehicleRepository vehicleRepository,
            ISaleOpportunityDomainService saleOpportunityDomainService)
        {
            _saleOpportunityRepository = saleOpportunityRepository;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
            _saleOpportunityDomainService = saleOpportunityDomainService;
        }

        public async Task<EntityCreatedResponse> Handle(CreateSaleOpportunityCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);

            if(customer is null)
            {
                throw new EntityNotFoundException("Cliente", request.CustomerId);
            }

            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);

            if(vehicle is null)
            {
                throw new EntityNotFoundException("Veículo", request.VehicleId);
            }

            await _saleOpportunityDomainService.ValidateNewSaleOpportunityAsync(
                customer,
                vehicle,
                cancellationToken);

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
