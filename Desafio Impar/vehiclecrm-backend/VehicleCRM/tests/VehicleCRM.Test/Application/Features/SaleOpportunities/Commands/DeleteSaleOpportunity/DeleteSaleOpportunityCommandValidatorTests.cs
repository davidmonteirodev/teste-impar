using FluentValidation.TestHelper;
using VehicleCRM.Application.Features.SaleOpportunities.Commands;

namespace VehicleCRM.Test.Application.Features.SaleOpportunities.Commands.DeleteSaleOpportunity;

public class DeleteSaleOpportunityCommandValidatorTests
{
    private readonly DeleteSaleOpportunityCommandValidator _validator;

    public DeleteSaleOpportunityCommandValidatorTests()
    {
        _validator = new DeleteSaleOpportunityCommandValidator();
    }

    [Fact]
    public void Validate_WithValidId_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new DeleteSaleOpportunityCommand(1);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithZeroId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteSaleOpportunityCommand(0);

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
        var command = new DeleteSaleOpportunityCommand(-1);

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
        var command = new DeleteSaleOpportunityCommand(999999);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
