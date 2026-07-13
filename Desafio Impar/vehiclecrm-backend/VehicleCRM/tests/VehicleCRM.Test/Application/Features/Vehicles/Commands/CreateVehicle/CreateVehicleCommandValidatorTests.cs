using FluentValidation.TestHelper;
using VehicleCRM.Application.Features.Vehicles.Commands;

namespace VehicleCRM.Test.Application.Features.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandValidatorTests
{
    private readonly CreateVehicleCommandValidator _validator;

    public CreateVehicleCommandValidatorTests()
    {
        _validator = new CreateVehicleCommandValidator();
    }

    [Fact]
    public void Validate_WithValidData_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateVehicleCommand(
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
    public void Validate_WithEmptyBrand_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateVehicleCommand("", "Corolla", 2023, 85000m, "Branco", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Brand)
            .WithErrorMessage("Marca é obrigatória.");
    }

    [Fact]
    public void Validate_WithNullBrand_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateVehicleCommand(null, "Corolla", 2023, 85000m, "Branco", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Brand);
    }

    [Fact]
    public void Validate_WithBrandExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var longBrand = new string('A', 151);
        var command = new CreateVehicleCommand(longBrand, "Corolla", 2023, 85000m, "Branco", 15000);

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
        var command = new CreateVehicleCommand("Toyota", "", 2023, 85000m, "Branco", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Model)
            .WithErrorMessage("Modelo é obrigatório.");
    }

    [Fact]
    public void Validate_WithNullModel_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateVehicleCommand("Toyota", null, 2023, 85000m, "Branco", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Model);
    }

    [Fact]
    public void Validate_WithModelExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var longModel = new string('A', 151);
        var command = new CreateVehicleCommand("Toyota", longModel, 2023, 85000m, "Branco", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Model)
            .WithErrorMessage("Modelo deve possuir no máximo 150 caracteres.");
    }

    [Fact]
    public void Validate_WithYearBefore1886_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateVehicleCommand("Toyota", "Corolla", 1885, 85000m, "Branco", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Year)
            .WithErrorMessage("Ano inválido.");
    }

    [Fact]
    public void Validate_WithYearExceedingNextYear_ShouldHaveValidationError()
    {
        // Arrange
        var invalidYear = DateTime.UtcNow.Year + 2;
        var command = new CreateVehicleCommand("Toyota", "Corolla", invalidYear, 85000m, "Branco", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Year)
            .WithErrorMessage("Ano inválido.");
    }

    [Fact]
    public void Validate_WithValidYearRange_ShouldNotHaveValidationError()
    {
        // Arrange
        var currentYear = DateTime.UtcNow.Year;
        var command1 = new CreateVehicleCommand("Toyota", "Corolla", 1886, 85000m, "Branco", 15000);
        var command2 = new CreateVehicleCommand("Toyota", "Corolla", currentYear, 85000m, "Branco", 15000);
        var command3 = new CreateVehicleCommand("Toyota", "Corolla", currentYear + 1, 85000m, "Branco", 15000);

        // Act & Assert
        _validator.TestValidate(command1).ShouldNotHaveValidationErrorFor(x => x.Year);
        _validator.TestValidate(command2).ShouldNotHaveValidationErrorFor(x => x.Year);
        _validator.TestValidate(command3).ShouldNotHaveValidationErrorFor(x => x.Year);
    }

    [Fact]
    public void Validate_WithZeroPrice_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateVehicleCommand("Toyota", "Corolla", 2023, 0m, "Branco", 15000);

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
        var command = new CreateVehicleCommand("Toyota", "Corolla", 2023, -1000m, "Branco", 15000);

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
        var command = new CreateVehicleCommand("Toyota", "Corolla", 2023, 85000m, "", 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Color)
            .WithErrorMessage("Cor é obrigatória.");
    }

    [Fact]
    public void Validate_WithNullColor_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateVehicleCommand("Toyota", "Corolla", 2023, 85000m, null, 15000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Color);
    }

    [Fact]
    public void Validate_WithColorExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var longColor = new string('A', 51);
        var command = new CreateVehicleCommand("Toyota", "Corolla", 2023, 85000m, longColor, 15000);

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
        var command = new CreateVehicleCommand("Toyota", "Corolla", 2023, 85000m, "Branco", -1);

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
        var command = new CreateVehicleCommand("Toyota", "Corolla", 2023, 85000m, "Branco", 0);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Mileage);
    }
}
