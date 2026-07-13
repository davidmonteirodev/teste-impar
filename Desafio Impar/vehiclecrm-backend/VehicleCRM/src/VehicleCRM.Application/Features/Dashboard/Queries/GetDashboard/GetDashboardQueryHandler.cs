using MediatR;
using VehicleCRM.Domain.Customers.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Enums;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.Vehicles.Enums;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Application.Features.Dashboard.Queries
{
    public sealed class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardResponse>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISaleOpportunityRepository _saleOpportunityRepository;

        public GetDashboardQueryHandler(
            IVehicleRepository vehicleRepository,
            ICustomerRepository customerRepository,
            ISaleOpportunityRepository saleOpportunityRepository)
        {
            _vehicleRepository = vehicleRepository;
            _customerRepository = customerRepository;
            _saleOpportunityRepository = saleOpportunityRepository;
        }

        public async Task<DashboardResponse> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            var vehicleCount = await _vehicleRepository.GetTotalCountAsync(cancellationToken);
            var customerCount = await _customerRepository.GetTotalCountAsync(cancellationToken);
            var opportunityCount = await _saleOpportunityRepository.GetTotalCountAsync(cancellationToken);
            var vehicleStatusCounts = await _vehicleRepository.GetCountByStatusAsync(cancellationToken);
            var opportunityStatusCounts = await _saleOpportunityRepository.GetCountByStatusAsync(cancellationToken);
            var soldVehiclesTotalValue = await _vehicleRepository.GetTotalSoldVehiclesValueAsync(cancellationToken);
            var soldVehicles = vehicleStatusCounts.GetValueOrDefault(VehicleSaleStatus.Sold, 0);

            var cards = new DashboardCardsResponse
            {
                Vehicles = vehicleCount,
                Customers = customerCount,
                Opportunities = opportunityCount,
                SoldVehiclesTotalValue = Math.Round(soldVehiclesTotalValue, 2)
            };

            var vehicleStatus = new List<StatusCountResponse>
            {
                new StatusCountResponse
                {
                    Status = "Disponível",
                    Count = vehicleStatusCounts.GetValueOrDefault(VehicleSaleStatus.Available, 0)
                },
                new StatusCountResponse
                {
                    Status = "Reservado",
                    Count = vehicleStatusCounts.GetValueOrDefault(VehicleSaleStatus.Reserved, 0)
                },
                new StatusCountResponse
                {
                    Status = "Vendido",
                    Count = soldVehicles
                }
            };

            var opportunityStatus = new List<StatusCountResponse>
            {
                new StatusCountResponse
                {
                    Status = "Lead",
                    Count = opportunityStatusCounts.GetValueOrDefault(SaleOpportunityStatus.NewLead, 0)
                },
                new StatusCountResponse
                {
                    Status = "Negociação",
                    Count = opportunityStatusCounts.GetValueOrDefault(SaleOpportunityStatus.InNegotiation, 0)
                },
                new StatusCountResponse
                {
                    Status = "Proposta",
                    Count = opportunityStatusCounts.GetValueOrDefault(SaleOpportunityStatus.ProposalSent, 0)
                },
                new StatusCountResponse
                {
                    Status = "Vendido",
                    Count = opportunityStatusCounts.GetValueOrDefault(SaleOpportunityStatus.Sold, 0)
                },
                new StatusCountResponse
                {
                    Status = "Perdido",
                    Count = opportunityStatusCounts.GetValueOrDefault(SaleOpportunityStatus.Lost, 0)
                }
            };

            return new DashboardResponse
            {
                Cards = cards,
                VehicleStatus = vehicleStatus,
                OpportunityStatus = opportunityStatus
            };
        }
    }
}
