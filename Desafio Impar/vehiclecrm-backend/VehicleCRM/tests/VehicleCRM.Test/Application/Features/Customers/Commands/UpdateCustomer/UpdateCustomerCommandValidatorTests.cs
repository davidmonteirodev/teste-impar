using FluentValidation.TestHelper;
using VehicleCRM.Application.Features.Customers.Commands;
using VehicleCRM.Domain.Customers.Enums;

namespace VehicleCRM.Test.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidatorTests
{
    private readonly UpdateCustomerCommandValidator _validator;

    public UpdateCustomerCommandValidatorTests()
    {
        _validator = new UpdateCustomerCommandValidator();
    }

    [Fact]
    public void Validate_WithValidData_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateCustomerCommand(
            Id: 1,
            Name: "João Silva",
            Phone: "(11) 98765-4321",
            MainInterest: CustomerMainInterest.Sedan
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
        var command = new UpdateCustomerCommand(0, "João Silva", "(11) 98765-4321", CustomerMainInterest.Sedan);

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
        var command = new UpdateCustomerCommand(-1, "João Silva", "(11) 98765-4321", CustomerMainInterest.Sedan);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id inválido.");
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateCustomerCommand(1, "", "(11) 98765-4321", CustomerMainInterest.Sedan);

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
        var command = new UpdateCustomerCommand(1, null, "(11) 98765-4321", CustomerMainInterest.Sedan);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Nome é obrigatório.");
    }

    [Fact]
    public void Validate_WithNameExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var longName = new string('A', 151);
        var command = new UpdateCustomerCommand(1, longName, "(11) 98765-4321", CustomerMainInterest.Sedan);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Nome deve possuir no máximo 150 caracteres.");
    }

    [Fact]
    public void Validate_WithEmptyPhone_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateCustomerCommand(1, "João Silva", "", CustomerMainInterest.Sedan);

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
        var command = new UpdateCustomerCommand(1, "João Silva", null, CustomerMainInterest.Sedan);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Phone)
            .WithErrorMessage("Telefone é obrigatório.");
    }

    [Fact]
    public void Validate_WithPhoneExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var longPhone = new string('1', 31);
        var command = new UpdateCustomerCommand(1, "João Silva", longPhone, CustomerMainInterest.Sedan);

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
        var command = new UpdateCustomerCommand(1, "João Silva", "(11) 98765-4321", (CustomerMainInterest)999);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MainInterest)
            .WithErrorMessage("Interesse principal inválido.");
    }

    [Fact]
    public void Validate_WithValidMainInterestValues_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command1 = new UpdateCustomerCommand(1, "João Silva", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var command2 = new UpdateCustomerCommand(1, "Maria Santos", "(11) 98765-4321", CustomerMainInterest.Suv);
        var command3 = new UpdateCustomerCommand(1, "Pedro Oliveira", "(11) 98765-4321", CustomerMainInterest.Hatch);
        var command4 = new UpdateCustomerCommand(1, "Ana Costa", "(11) 98765-4321", CustomerMainInterest.Utility);

        // Act
        var result1 = _validator.TestValidate(command1);
        var result2 = _validator.TestValidate(command2);
        var result3 = _validator.TestValidate(command3);
        var result4 = _validator.TestValidate(command4);

        // Assert
        result1.ShouldNotHaveAnyValidationErrors();
        result2.ShouldNotHaveAnyValidationErrors();
        result3.ShouldNotHaveAnyValidationErrors();
        result4.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithNameAtMaxLength_ShouldNotHaveValidationError()
    {
        // Arrange
        var nameAtMaxLength = new string('A', 150);
        var command = new UpdateCustomerCommand(1, nameAtMaxLength, "(11) 98765-4321", CustomerMainInterest.Sedan);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithPhoneAtMaxLength_ShouldNotHaveValidationError()
    {
        // Arrange
        var phoneAtMaxLength = new string('1', 30);
        var command = new UpdateCustomerCommand(1, "João Silva", phoneAtMaxLength, CustomerMainInterest.Sedan);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Phone);
    }
}
