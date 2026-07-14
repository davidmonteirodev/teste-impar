using FluentValidation;

namespace VehicleCRM.Application.Features.Customers.Commands
{
    public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(150).WithMessage("Nome deve possuir no máximo 150 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório.")
                .EmailAddress().WithMessage("Email inválido.")
                .MaximumLength(150).WithMessage("Email deve possuir no máximo 150 caracteres.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefone é obrigatório.")
                .MaximumLength(30).WithMessage("Telefone deve possuir no máximo 30 caracteres.");

            RuleFor(x => x.MainInterest)
                .IsInEnum()
                .WithMessage("Interesse principal inválido.");
        }
    }
}
