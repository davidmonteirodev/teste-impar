using FluentValidation;

namespace VehicleCRM.Application.Features.Customers.Commands
{
    public sealed class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id inválido.");
        }
    }
}
