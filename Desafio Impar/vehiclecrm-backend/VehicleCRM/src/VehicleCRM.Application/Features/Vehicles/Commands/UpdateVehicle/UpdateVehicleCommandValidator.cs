using FluentValidation;

namespace VehicleCRM.Application.Features.Vehicles.Commands
{
    public sealed class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand>
    {
        public UpdateVehicleCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Brand)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Model)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Year)
                .InclusiveBetween(1886, DateTime.UtcNow.Year + 1);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.Color)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Mileage)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Status)
                .IsInEnum();
        }
    }
}
