using FluentValidation.TestHelper;
using VehicleCRM.Application.Features.SaleOpportunities.Commands;

namespace VehicleCRM.Test.Application.Features.SaleOpportunities.Commands.CreateSaleOpportunity;

public class CreateSaleOpportunityCommandValidatorTests
{
    private readonly CreateSaleOpportunityCommandValidator _validator;

    public CreateSaleOpportunityCommandValidatorTests()
    {
        _validator = new CreateSaleOpportunityCommandValidator();
    }

    [Fact]
    public void Validate_WithValidData_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(
            CustomerId: 1,
            VehicleId: 1,
            ProposedValue: 80000m,
            Notes: "Cliente interessado"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithZeroCustomerId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(0, 1, 80000m, "Cliente interessado");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CustomerId)
            .WithErrorMessage("Id do cliente inválido.");
    }

    [Fact]
    public void Validate_WithNegativeCustomerId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(-1, 1, 80000m, "Cliente interessado");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CustomerId)
            .WithErrorMessage("Id do cliente inválido.");
    }

    [Fact]
    public void Validate_WithZeroVehicleId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(1, 0, 80000m, "Cliente interessado");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.VehicleId)
            .WithErrorMessage("Id do veículo inválido.");
    }

    [Fact]
    public void Validate_WithNegativeVehicleId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(1, -1, 80000m, "Cliente interessado");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.VehicleId)
            .WithErrorMessage("Id do veículo inválido.");
    }

    [Fact]
    public void Validate_WithZeroProposedValue_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(1, 1, 0m, "Cliente interessado");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProposedValue)
            .WithErrorMessage("Valor proposto inválido.");
    }

    [Fact]
    public void Validate_WithNegativeProposedValue_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(1, 1, -1000m, "Cliente interessado");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProposedValue)
            .WithErrorMessage("Valor proposto inválido.");
    }

    [Fact]
    public void Validate_WithNullNotes_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(1, 1, 80000m, null);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyNotes_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(1, 1, 80000m, "");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithMultipleErrors_ShouldHaveAllValidationErrors()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(0, 0, 0m, "Cliente interessado");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CustomerId);
        result.ShouldHaveValidationErrorFor(x => x.VehicleId);
        result.ShouldHaveValidationErrorFor(x => x.ProposedValue);
    }

    [Fact]
    public void Validate_WithLargePositiveValues_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(999999, 999999, 9999999.99m, "Cliente interessado");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
