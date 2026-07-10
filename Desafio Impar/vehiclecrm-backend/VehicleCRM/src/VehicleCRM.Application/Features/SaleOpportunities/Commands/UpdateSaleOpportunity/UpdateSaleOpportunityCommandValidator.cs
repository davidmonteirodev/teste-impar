using FluentValidation;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed class UpdateSaleOpportunityCommandValidator : AbstractValidator<UpdateSaleOpportunityCommand>
    {
        public UpdateSaleOpportunityCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.CustomerId)
                .GreaterThan(0);

            RuleFor(x => x.VehicleId)
                .GreaterThan(0);

            RuleFor(x => x.Status)
                .IsInEnum();

            RuleFor(x => x.ProposedValue)
                .GreaterThan(0);

            RuleFor(x => x.Notes)
                .NotEmpty()
                .MaximumLength(2000);
        }
    }
}
