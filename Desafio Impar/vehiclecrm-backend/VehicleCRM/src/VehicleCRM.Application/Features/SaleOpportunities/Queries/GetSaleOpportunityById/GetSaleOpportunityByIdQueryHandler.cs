using MediatR;
using VehicleCRM.Domain.SaleOpportunities.Repositories;

namespace VehicleCRM.Application.Features.SaleOpportunities.Queries
{
    public sealed class GetSaleOpportunityByIdQueryHandler : IRequestHandler<GetSaleOpportunityByIdQuery, SaleOpportunityResponse>
    {
        private readonly ISaleOpportunityRepository _saleOpportunityRepository;

        public GetSaleOpportunityByIdQueryHandler(ISaleOpportunityRepository saleOpportunityRepository)
        {
            _saleOpportunityRepository = saleOpportunityRepository;
        }

        public async Task<SaleOpportunityResponse> Handle(GetSaleOpportunityByIdQuery request, CancellationToken cancellationToken)
        {
            var saleOpportunity = await _saleOpportunityRepository.GetByIdAsync(request.Id, cancellationToken);

            return saleOpportunity.ToResponse();
        }
    }
}
