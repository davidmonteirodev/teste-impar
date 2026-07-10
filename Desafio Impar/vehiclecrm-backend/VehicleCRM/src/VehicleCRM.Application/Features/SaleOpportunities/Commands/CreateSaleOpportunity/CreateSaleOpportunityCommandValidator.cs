using FluentValidation;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed class CreateSaleOpportunityCommandValidator : AbstractValidator<CreateSaleOpportunityCommand>
    {
        public CreateSaleOpportunityCommandValidator()
        {
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
