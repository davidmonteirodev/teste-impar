using FluentValidation;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed class CreateSaleOpportunityCommandValidator : AbstractValidator<CreateSaleOpportunityCommand>
    {
        public CreateSaleOpportunityCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("Id do cliente inválido.");

            RuleFor(x => x.VehicleId)
                .GreaterThan(0)
                .WithMessage("Id do veículo inválido.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Status inválido.");

            RuleFor(x => x.ProposedValue)
                .GreaterThan(0)
                .WithMessage("Valor proposto inválido.");

            RuleFor(x => x.Notes)
                .NotEmpty().WithMessage("Observações são obrigatórias.")
                .MaximumLength(2000).WithMessage("Observações devem possuir no máximo 2000 caracteres.");
        }
    }
}
