using FluentValidation;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed class DeleteSaleOpportunityCommandValidator : AbstractValidator<DeleteSaleOpportunityCommand>
    {
        public DeleteSaleOpportunityCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
