using MediatR;
using VehicleCRM.Application.Common.Exceptions;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Domain.Common.UnitOfWork;
using VehicleCRM.Domain.Customers.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Entities;
using VehicleCRM.Domain.SaleOpportunities.Enums;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Services;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed class CreateSaleOpportunityCommandHandler : BaseSaleOpportunityCommandHandler, IRequestHandler<CreateSaleOpportunityCommand, EntityCreatedResponse>
    {
        private readonly ISaleOpportunityRepository _saleOpportunityRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISaleOpportunityDomainService _saleOpportunityDomainService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSaleOpportunityCommandHandler(
            ISaleOpportunityRepository saleOpportunityRepository,
            ICustomerRepository customerRepository,
            IVehicleRepository vehicleRepository,
            ISaleOpportunityDomainService saleOpportunityDomainService,
            IUnitOfWork unitOfWork)
            : base(vehicleRepository)
        {
            _saleOpportunityRepository = saleOpportunityRepository;
            _customerRepository = customerRepository;
            _saleOpportunityDomainService = saleOpportunityDomainService;
            _unitOfWork = unitOfWork;
        }

        public async Task<EntityCreatedResponse> Handle(CreateSaleOpportunityCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);

            if (customer is null)
                throw new EntityNotFoundException("Cliente", request.CustomerId);

            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);

            if (vehicle is null)
                throw new EntityNotFoundException("Veículo", request.VehicleId);

            await _saleOpportunityDomainService.ValidateNewSaleOpportunityAsync(
                customer,
                vehicle,
                cancellationToken);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var saleOpportunity = SaleOpportunity.Create(
                customer.Id,
                vehicle.Id,
                request.ProposedValue,
                request.Notes);

            await _saleOpportunityRepository.InsertAsync(saleOpportunity, cancellationToken);

            await UpdateVehicleStatusBasedOnSaleOpportunityAsync(vehicle, SaleOpportunityStatus.NewLead, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return new EntityCreatedResponse(saleOpportunity.Id, saleOpportunity.CreateDate);
        }
    }
}
