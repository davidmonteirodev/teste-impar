using VehicleCRM.Domain.Customers.Exceptions;

namespace VehicleCRM.Test.Domain.Customers.Exceptions;

public class DuplicateCustomerEmailExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetCorrectMessage()
    {
        // Arrange
        var email = "teste@example.com";

        // Act
        var exception = new DuplicateCustomerEmailException(email);

        // Assert
        exception.Message.Should().Be("Já existe um cliente cadastrado com o email 'teste@example.com'.");
    }

    [Fact]
    public void Constructor_WithDifferentEmail_ShouldIncludeEmailInMessage()
    {
        // Arrange
        var email = "joao.silva@example.com";

        // Act
        var exception = new DuplicateCustomerEmailException(email);

        // Assert
        exception.Message.Should().Contain(email);
    }

    [Fact]
    public void Exception_ShouldBeThrowable()
    {
        // Arrange
        var email = "teste@example.com";
        Action act = () => throw new DuplicateCustomerEmailException(email);

        // Act & Assert
        act.Should().Throw<DuplicateCustomerEmailException>()
            .WithMessage("*teste@example.com*");
    }

    [Fact]
    public void Exception_ShouldBeDomainException()
    {
        // Act
        var exception = new DuplicateCustomerEmailException("teste@example.com");

        // Assert
        exception.Should().BeAssignableTo<VehicleCRM.Domain.Common.Exceptions.DomainException>();
    }

    [Fact]
    public void Constructor_WithEmptyEmail_ShouldStillCreateException()
    {
        // Arrange
        var email = "";

        // Act
        var exception = new DuplicateCustomerEmailException(email);

        // Assert
        exception.Message.Should().Be("Já existe um cliente cadastrado com o email ''.");
    }
}
