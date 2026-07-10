using FluentValidation;

namespace VehicleCRM.Application.Features.Customers.Commands
{
    public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150)
                .WithState(x => new { Message = "Nome é obrigatório e deve possuir no máximo 150 caracteres." });

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(150)
                .WithMessage("Email é obrigatório e deve possuir no máximo 150 caracteres.");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MaximumLength(30)
                .WithMessage("Telefone é obrigatório e deve possuir no máximo 30 caracteres.");

            RuleFor(x => x.MainInterest)
                .IsInEnum()
                .WithMessage("Interesse principal inválido.");
        }
    }
}
