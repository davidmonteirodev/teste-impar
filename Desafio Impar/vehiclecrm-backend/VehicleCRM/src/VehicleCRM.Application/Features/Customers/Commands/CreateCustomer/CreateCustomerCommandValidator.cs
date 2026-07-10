using FluentValidation;

namespace VehicleCRM.Application.Features.Customers.Commands
{
    public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
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
