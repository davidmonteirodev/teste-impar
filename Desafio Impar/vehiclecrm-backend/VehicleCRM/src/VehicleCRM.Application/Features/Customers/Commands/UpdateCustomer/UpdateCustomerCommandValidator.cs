using FluentValidation;

namespace VehicleCRM.Application.Features.Customers.Commands
{
    public sealed class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(150);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.MainInterest)
                .IsInEnum();
        }
    }
}
