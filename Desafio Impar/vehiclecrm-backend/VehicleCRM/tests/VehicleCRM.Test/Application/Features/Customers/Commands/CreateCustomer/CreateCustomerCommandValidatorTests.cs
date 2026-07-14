using FluentValidation.TestHelper;
using VehicleCRM.Application.Features.Customers.Commands;
using VehicleCRM.Domain.Customers.Enums;

namespace VehicleCRM.Test.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidatorTests
{
    private readonly CreateCustomerCommandValidator _validator;

    public CreateCustomerCommandValidatorTests()
    {
        _validator = new CreateCustomerCommandValidator();
    }

    [Fact]
    public void Validate_WithValidData_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            Name: "João Silva",
            Email: "joao@example.com",
            Phone: "(11) 98765-4321",
            MainInterest: CustomerMainInterest.Sedan
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            "",
            "joao@example.com",
            "11987654321",
            CustomerMainInterest.Sedan
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Nome é obrigatório.");
    }

    [Fact]
    public void Validate_WithNullName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            null,
            "joao@example.com",
            "11987654321",
            CustomerMainInterest.Sedan
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNameExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var longName = new string('A', 151);
        var command = new CreateCustomerCommand(
            longName,
            "joao@example.com",
            "11987654321",
            CustomerMainInterest.Sedan
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Nome deve possuir no máximo 150 caracteres.");
    }

    [Fact]
    public void Validate_WithEmptyEmail_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            "João Silva",
            "",
            "11987654321",
            CustomerMainInterest.Sedan
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email é obrigatório.");
    }

    [Fact]
    public void Validate_WithNullEmail_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            "João Silva",
            null,
            "11987654321",
            CustomerMainInterest.Sedan
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WithInvalidEmail_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            "João Silva",
            "invalid-email",
            "11987654321",
            CustomerMainInterest.Sedan
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email inválido.");
    }

    [Fact]
    public void Validate_WithEmailExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var longEmail = new string('a', 140) + "@example.com"; // 153 caracteres
        var command = new CreateCustomerCommand(
            "João Silva",
            longEmail,
            "11987654321",
            CustomerMainInterest.Sedan
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email deve possuir no máximo 150 caracteres.");
    }

    [Fact]
    public void Validate_WithEmptyPhone_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            "João Silva",
            "joao@example.com",
            "",
            CustomerMainInterest.Sedan
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Phone)
            .WithErrorMessage("Telefone é obrigatório.");
    }

    [Fact]
    public void Validate_WithNullPhone_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            "João Silva",
            "joao@example.com",
            null,
            CustomerMainInterest.Sedan
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void Validate_WithPhoneExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var longPhone = new string('1', 31);
        var command = new CreateCustomerCommand(
            "João Silva",
            "joao@example.com",
            longPhone,
            CustomerMainInterest.Sedan
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Phone)
            .WithErrorMessage("Telefone deve possuir no máximo 30 caracteres.");
    }

    [Fact]
    public void Validate_WithInvalidMainInterest_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            "João Silva",
            "joao@example.com",
            "11987654321",
            (CustomerMainInterest)999
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MainInterest)
            .WithErrorMessage("Interesse principal inválido.");
    }

    [Fact]
    public void Validate_WithAllValidMainInterests_ShouldNotHaveValidationErrors()
    {
        // Arrange & Act & Assert
        var interests = new[]
        {
            CustomerMainInterest.Suv,
            CustomerMainInterest.Hatch,
            CustomerMainInterest.Sedan,
            CustomerMainInterest.Utility,
            CustomerMainInterest.UsedCar,
            CustomerMainInterest.NewCar
        };

        foreach (var interest in interests)
        {
            var command = new CreateCustomerCommand(
                "João Silva",
                "joao@example.com",
                "11987654321",
                interest
            );

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.MainInterest);
        }
    }
}
