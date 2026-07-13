using FluentValidation.TestHelper;
using VehicleCRM.Application.Features.Customers.Commands;

namespace VehicleCRM.Test.Application.Features.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandValidatorTests
{
    private readonly DeleteCustomerCommandValidator _validator;

    public DeleteCustomerCommandValidatorTests()
    {
        _validator = new DeleteCustomerCommandValidator();
    }

    [Fact]
    public void Validate_WithValidId_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new DeleteCustomerCommand(1);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithZeroId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteCustomerCommand(0);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id inválido.");
    }

    [Fact]
    public void Validate_WithNegativeId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteCustomerCommand(-1);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id inválido.");
    }

    [Fact]
    public void Validate_WithLargePositiveId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new DeleteCustomerCommand(999999);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
