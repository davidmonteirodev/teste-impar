using FluentValidation.TestHelper;
using VehicleCRM.Application.Features.Vehicles.Commands;

namespace VehicleCRM.Test.Application.Features.Vehicles.Commands.UpdateVehicle;

public class UpdateVehicleCommandValidatorTests
{
    private readonly UpdateVehicleCommandValidator _validator;

    public UpdateVehicleCommandValidatorTests()
    {
        _validator = new UpdateVehicleCommandValidator();
    }

    [Fact]
    public void Validate_WithValidData_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateVehicleCommand(
            Id: 1,
            Brand: "Toyota",
            Model: "Corolla",
            Year: 2023,
            Price: 85000m,
            Color: "Branco",
            Mileage: 15000
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
        var command = new UpdateVehicleCommand(0, "Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

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
        var command = new UpdateVehicleCommand(-1, "Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id inválido.");
    }

    [Fact]
    public void Validate_WithEmptyBrand_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateVehicleCommand(1, "", "Corolla", 2023, 85000m, "Branco", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Brand)
            .WithErrorMessage("Marca é obrigatória.");
    }

    [Fact]
    public void Validate_WithBrandExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var longBrand = new string('A', 151);
        var command = new UpdateVehicleCommand(1, longBrand, "Corolla", 2023, 85000m, "Branco", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Brand)
            .WithErrorMessage("Marca deve possuir no máximo 150 caracteres.");
    }

    [Fact]
    public void Validate_WithEmptyModel_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateVehicleCommand(1, "Toyota", "", 2023, 85000m, "Branco", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Model)
            .WithErrorMessage("Modelo é obrigatório.");
    }

    [Fact]
    public void Validate_WithModelExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var longModel = new string('A', 151);
        var command = new UpdateVehicleCommand(1, "Toyota", longModel, 2023, 85000m, "Branco", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Model)
            .WithErrorMessage("Modelo deve possuir no máximo 150 caracteres.");
    }

    [Fact]
    public void Validate_WithInvalidYear_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateVehicleCommand(1, "Toyota", "Corolla", 1885, 85000m, "Branco", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Year)
            .WithErrorMessage("Ano inválido.");
    }

    [Fact]
    public void Validate_WithZeroPrice_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateVehicleCommand(1, "Toyota", "Corolla", 2023, 0m, "Branco", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price)
            .WithErrorMessage("Preço inválido.");
    }

    [Fact]
    public void Validate_WithNegativePrice_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateVehicleCommand(1, "Toyota", "Corolla", 2023, -1000m, "Branco", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price)
            .WithErrorMessage("Preço inválido.");
    }

    [Fact]
    public void Validate_WithEmptyColor_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateVehicleCommand(1, "Toyota", "Corolla", 2023, 85000m, "", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Color)
            .WithErrorMessage("Cor é obrigatória.");
    }

    [Fact]
    public void Validate_WithColorExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var longColor = new string('A', 51);
        var command = new UpdateVehicleCommand(1, "Toyota", "Corolla", 2023, 85000m, longColor, 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Color)
            .WithErrorMessage("Cor deve possuir no máximo 50 caracteres.");
    }

    [Fact]
    public void Validate_WithNegativeMileage_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateVehicleCommand(1, "Toyota", "Corolla", 2023, 85000m, "Branco", -1);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Mileage)
            .WithErrorMessage("Quilometragem inválida.");
    }

    [Fact]
    public void Validate_WithZeroMileage_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new UpdateVehicleCommand(1, "Toyota", "Corolla", 2023, 85000m, "Branco", 0);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Mileage);
    }
}
