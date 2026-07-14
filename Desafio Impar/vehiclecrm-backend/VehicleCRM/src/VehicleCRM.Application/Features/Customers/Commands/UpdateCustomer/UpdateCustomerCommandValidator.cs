using FluentValidation;

namespace VehicleCRM.Application.Features.Customers.Commands
{
    public sealed class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id inválido.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(150).WithMessage("Nome deve possuir no máximo 150 caracteres.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefone é obrigatório.")
                .MaximumLength(30).WithMessage("Telefone deve possuir no máximo 30 caracteres.");

            RuleFor(x => x.MainInterest)
                .IsInEnum()
                .WithMessage("Interesse principal inválido.");
        }
    }
}
