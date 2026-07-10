using FluentValidation;

namespace VehicleCRM.Application.Features.Vehicles.Commands
{
    public sealed class DeleteVehicleCommandValidator : AbstractValidator<DeleteVehicleCommand>
    {
        public DeleteVehicleCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
