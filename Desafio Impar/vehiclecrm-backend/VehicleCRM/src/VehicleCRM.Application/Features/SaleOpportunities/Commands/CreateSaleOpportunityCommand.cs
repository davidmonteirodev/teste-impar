using MediatR;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Domain.Entities;
using VehicleCRM.Domain.Enums;
using VehicleCRM.Domain.Interfaces.Repositories;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed record CreateSaleOpportunityCommand(
        long CustomerId,
        long VehicleId,
        SaleOpportunityStatus Status,
        decimal ProposedValue,
        string Notes) : IRequest<EntityCreatedResponse>;

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
