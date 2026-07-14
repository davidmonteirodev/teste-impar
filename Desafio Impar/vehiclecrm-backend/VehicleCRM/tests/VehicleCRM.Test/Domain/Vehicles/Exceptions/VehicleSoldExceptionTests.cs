using VehicleCRM.Domain.Vehicles.Exceptions;

namespace VehicleCRM.Test.Domain.Vehicles.Exceptions;

public class VehicleSoldExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetCorrectMessage()
    {
        // Act
        var exception = new VehicleSoldException();

        // Assert
        exception.Message.Should().Be("Não é possível editar um veículo vendido.");
    }

    [Fact]
    public void Exception_ShouldBeThrowable()
    {
        // Arrange
        Action act = () => throw new VehicleSoldException();

        // Act & Assert
        act.Should().Throw<VehicleSoldException>()
            .WithMessage("Não é possível editar um veículo vendido.");
    }

    [Fact]
    public void Exception_ShouldBeDomainException()
    {
        // Act
        var exception = new VehicleSoldException();

        // Assert
        exception.Should().BeAssignableTo<VehicleCRM.Domain.Common.Exceptions.DomainException>();
    }
}
