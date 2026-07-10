using FluentValidation;

namespace VehicleCRM.Application.Features.Vehicles.Commands
{
    public sealed class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
    {
        public CreateVehicleCommandValidator()
        {
            RuleFor(x => x.Brand)
                .NotEmpty()
                .MaximumLength(150)
                .WithMessage("Marca é obrigatória e deve possuir no máximo 150 caracteres.");

            RuleFor(x => x.Model)
                .NotEmpty()
                .MaximumLength(150)
                .WithMessage("Modelo é obrigatório e deve possuir no máximo 150 caracteres.");

            RuleFor(x => x.Year)
                .InclusiveBetween(1886, DateTime.UtcNow.Year + 1)
                .WithMessage("Ano inválido.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Preço inválido.");

            RuleFor(x => x.Color)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Cor é obrigatória e deve possuir no máximo 50 caracteres.");

            RuleFor(x => x.Mileage)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Quilometragem inválida.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Status inválido.");
        }
    }
}
