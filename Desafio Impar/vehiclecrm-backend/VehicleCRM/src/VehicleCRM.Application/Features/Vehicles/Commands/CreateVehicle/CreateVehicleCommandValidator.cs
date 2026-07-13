using FluentValidation;

namespace VehicleCRM.Application.Features.Vehicles.Commands
{
    public sealed class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
    {
        public CreateVehicleCommandValidator()
        {
            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Marca é obrigatória.")
                .MaximumLength(150).WithMessage("Marca deve possuir no máximo 150 caracteres.");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Modelo é obrigatório.")
                .MaximumLength(150).WithMessage("Modelo deve possuir no máximo 150 caracteres.");

            RuleFor(x => x.Year)
                .InclusiveBetween(1886, DateTime.UtcNow.Year + 1)
                .WithMessage("Ano inválido.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Preço inválido.");

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("Cor é obrigatória.")
                .MaximumLength(50).WithMessage("Cor deve possuir no máximo 50 caracteres.");

            RuleFor(x => x.Mileage)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Quilometragem inválida.");
        }
    }
}
