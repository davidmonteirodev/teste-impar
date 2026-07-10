using MediatR;
using VehicleCRM.Domain.Interfaces.Repositories;

namespace VehicleCRM.Application.Features.SaleOpportunities.Queries
{
    public sealed record GetSaleOpportunitiesQuery : IRequest<IReadOnlyCollection<SaleOpportunityResponse>>;

    public sealed class GetSaleOpportunitiesQueryHandler : IRequestHandler<GetSaleOpportunitiesQuery, IReadOnlyCollection<SaleOpportunityResponse>>
    {
        private readonly ISaleOpportunityRepository _saleOpportunityRepository;

        public GetSaleOpportunitiesQueryHandler(ISaleOpportunityRepository saleOpportunityRepository)
        {
            _saleOpportunityRepository = saleOpportunityRepository;
        }

        public async Task<IReadOnlyCollection<SaleOpportunityResponse>> Handle(GetSaleOpportunitiesQuery request, CancellationToken cancellationToken)
        {
            var saleOpportunities = await _saleOpportunityRepository.GetAllAsync(cancellationToken);

            return saleOpportunities
                .Select(saleOpportunity => saleOpportunity.ToResponse())
                .ToArray();
        }
    }
}
