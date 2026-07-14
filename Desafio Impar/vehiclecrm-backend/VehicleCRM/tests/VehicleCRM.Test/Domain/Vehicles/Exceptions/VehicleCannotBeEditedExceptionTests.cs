using VehicleCRM.Domain.Vehicles.Exceptions;

namespace VehicleCRM.Test.Domain.Vehicles.Exceptions;

public class VehicleCannotBeEditedExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetCorrectMessage()
    {
        // Arrange
        var status = "Vendido";

        // Act
        var exception = new VehicleCannotBeEditedException(status);

        // Assert
        exception.Message.Should().Be("Não é possível editar um veículo com status 'Vendido'.");
    }

    [Fact]
    public void Constructor_ShouldIncludeStatusInMessage()
    {
        // Arrange
        var status = "Reservado";

        // Act
        var exception = new VehicleCannotBeEditedException(status);

        // Assert
        exception.Message.Should().Contain(status);
    }

    [Fact]
    public void Exception_ShouldBeThrowable()
    {
        // Arrange
        var status = "Vendido";
        Action act = () => throw new VehicleCannotBeEditedException(status);

        // Act & Assert
        act.Should().Throw<VehicleCannotBeEditedException>()
            .WithMessage("*Vendido*");
    }

    [Fact]
    public void Exception_ShouldBeDomainException()
    {
        // Act
        var exception = new VehicleCannotBeEditedException("Vendido");

        // Assert
        exception.Should().BeAssignableTo<VehicleCRM.Domain.Common.Exceptions.DomainException>();
    }

    [Fact]
    public void Constructor_WithDifferentStatuses_ShouldCreateExceptionWithCorrectMessage()
    {
        // Arrange
        var status1 = "Disponível";
        var status2 = "Reservado";
        var status3 = "Vendido";

        // Act
        var exception1 = new VehicleCannotBeEditedException(status1);
        var exception2 = new VehicleCannotBeEditedException(status2);
        var exception3 = new VehicleCannotBeEditedException(status3);

        // Assert
        exception1.Message.Should().Contain(status1);
        exception2.Message.Should().Contain(status2);
        exception3.Message.Should().Contain(status3);
    }
}
