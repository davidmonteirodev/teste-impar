using MediatR;
using VehicleCRM.Domain.Interfaces.Repositories;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed class DeleteSaleOpportunityCommandHandler : IRequestHandler<DeleteSaleOpportunityCommand>
    {
        private readonly ISaleOpportunityRepository _saleOpportunityRepository;

        public DeleteSaleOpportunityCommandHandler(ISaleOpportunityRepository saleOpportunityRepository)
        {
            _saleOpportunityRepository = saleOpportunityRepository;
        }

        public async Task Handle(DeleteSaleOpportunityCommand request, CancellationToken cancellationToken)
        {
            await _saleOpportunityRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
