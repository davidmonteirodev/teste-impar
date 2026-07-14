using MediatR;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Repositories.Criteria;

namespace VehicleCRM.Application.Features.SaleOpportunities.Queries
{
    public sealed class GetSaleOpportunitiesQueryHandler : IRequestHandler<GetSaleOpportunitiesQuery, PagedResult<SaleOpportunityResponse>>
    {
        private readonly ISaleOpportunityRepository _saleOpportunityRepository;

        public GetSaleOpportunitiesQueryHandler(ISaleOpportunityRepository saleOpportunityRepository)
        {
            _saleOpportunityRepository = saleOpportunityRepository;
        }

        public async Task<PagedResult<SaleOpportunityResponse>> Handle(GetSaleOpportunitiesQuery request, CancellationToken cancellationToken)
        {
            var criteria = new SaleOpportunitySearchCriteria
            {
                Page = request.Page,
                PageSize = request.PageSize,
                CustomerName = request.CustomerName,
                VehicleModel = request.VehicleModel,
                Status = request.Status,
                ProposedValueFrom = request.ProposedValueFrom,
                ProposedValueTo = request.ProposedValueTo,
                CreateDateFrom = request.CreateDateFrom,
                CreateDateTo = request.CreateDateTo
            };

            var (saleOpportunities, totalCount) = await _saleOpportunityRepository.GetPagedAsync(criteria, cancellationToken);

            var saleOpportunityResponses = saleOpportunities
                .Select(saleOpportunity => saleOpportunity.ToResponse())
                .ToArray();

            return new PagedResult<SaleOpportunityResponse>(
                saleOpportunityResponses,
                totalCount,
                request.Page,
                request.PageSize);
        }
    }
}
