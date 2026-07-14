using FluentValidation.TestHelper;
using VehicleCRM.Application.Features.SaleOpportunities.Commands;
using VehicleCRM.Domain.SaleOpportunities.Enums;

namespace VehicleCRM.Test.Application.Features.SaleOpportunities.Commands.UpdateSaleOpportunity;

public class UpdateSaleOpportunityCommandValidatorTests
{
    private readonly UpdateSaleOpportunityCommandValidator _validator;

    public UpdateSaleOpportunityCommandValidatorTests()
    {
        _validator = new UpdateSaleOpportunityCommandValidator();
    }

    [Fact]
    public void Validate_WithValidData_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateSaleOpportunityCommand(
            Id: 1,
            CustomerId: 1,
            VehicleId: 1,
            Status: SaleOpportunityStatus.InNegotiation,
            ProposedValue: 80000m,
            Notes: "Cliente interessado"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithZeroId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateSaleOpportunityCommand(0, 1, 1, SaleOpportunityStatus.InNegotiation, 80000m, "Notas");

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
        var command = new UpdateSaleOpportunityCommand(-1, 1, 1, SaleOpportunityStatus.InNegotiation, 80000m, "Notas");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id inválido.");
    }

    [Fact]
    public void Validate_WithZeroCustomerId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateSaleOpportunityCommand(1, 0, 1, SaleOpportunityStatus.InNegotiation, 80000m, "Notas");

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
        var command = new UpdateSaleOpportunityCommand(1, -1, 1, SaleOpportunityStatus.InNegotiation, 80000m, "Notas");

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
        var command = new UpdateSaleOpportunityCommand(1, 1, 0, SaleOpportunityStatus.InNegotiation, 80000m, "Notas");

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
        var command = new UpdateSaleOpportunityCommand(1, 1, -1, SaleOpportunityStatus.InNegotiation, 80000m, "Notas");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.VehicleId)
            .WithErrorMessage("Id do veículo inválido.");
    }

    [Fact]
    public void Validate_WithInvalidStatus_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateSaleOpportunityCommand(1, 1, 1, (SaleOpportunityStatus)999, 80000m, "Notas");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Status)
            .WithErrorMessage("Status inválido.");
    }

    [Fact]
    public void Validate_WithZeroProposedValue_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateSaleOpportunityCommand(1, 1, 1, SaleOpportunityStatus.InNegotiation, 0m, "Notas");

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
        var command = new UpdateSaleOpportunityCommand(1, 1, 1, SaleOpportunityStatus.InNegotiation, -1000m, "Notas");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProposedValue)
            .WithErrorMessage("Valor proposto inválido.");
    }

    [Fact]
    public void Validate_WithValidStatusValues_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command1 = new UpdateSaleOpportunityCommand(1, 1, 1, SaleOpportunityStatus.NewLead, 80000m, "Notas");
        var command2 = new UpdateSaleOpportunityCommand(1, 1, 1, SaleOpportunityStatus.InNegotiation, 80000m, "Notas");
        var command3 = new UpdateSaleOpportunityCommand(1, 1, 1, SaleOpportunityStatus.ProposalSent, 80000m, "Notas");
        var command4 = new UpdateSaleOpportunityCommand(1, 1, 1, SaleOpportunityStatus.Sold, 80000m, "Notas");
        var command5 = new UpdateSaleOpportunityCommand(1, 1, 1, SaleOpportunityStatus.Lost, 80000m, "Notas");

        // Act
        var result1 = _validator.TestValidate(command1);
        var result2 = _validator.TestValidate(command2);
        var result3 = _validator.TestValidate(command3);
        var result4 = _validator.TestValidate(command4);
        var result5 = _validator.TestValidate(command5);

        // Assert
        result1.ShouldNotHaveAnyValidationErrors();
        result2.ShouldNotHaveAnyValidationErrors();
        result3.ShouldNotHaveAnyValidationErrors();
        result4.ShouldNotHaveAnyValidationErrors();
        result5.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithNullNotes_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new UpdateSaleOpportunityCommand(1, 1, 1, SaleOpportunityStatus.InNegotiation, 80000m, null);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyNotes_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new UpdateSaleOpportunityCommand(1, 1, 1, SaleOpportunityStatus.InNegotiation, 80000m, "");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithMultipleErrors_ShouldHaveAllValidationErrors()
    {
        // Arrange
        var command = new UpdateSaleOpportunityCommand(0, 0, 0, (SaleOpportunityStatus)999, 0m, "Notas");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
        result.ShouldHaveValidationErrorFor(x => x.CustomerId);
        result.ShouldHaveValidationErrorFor(x => x.VehicleId);
        result.ShouldHaveValidationErrorFor(x => x.Status);
        result.ShouldHaveValidationErrorFor(x => x.ProposedValue);
    }
}
