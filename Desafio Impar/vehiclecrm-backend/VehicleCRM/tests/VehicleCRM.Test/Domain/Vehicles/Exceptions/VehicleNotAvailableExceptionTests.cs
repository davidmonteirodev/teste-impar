using VehicleCRM.Domain.Vehicles.Exceptions;

namespace VehicleCRM.Test.Domain.Vehicles.Exceptions;

public class VehicleNotAvailableExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetCorrectMessage()
    {
        // Arrange
        var vehicleModel = "Toyota Corolla";

        // Act
        var exception = new VehicleNotAvailableException(vehicleModel);

        // Assert
        exception.Message.Should().Be("O veículo Toyota Corolla não está disponível para venda.");
    }

    [Fact]
    public void Constructor_ShouldIncludeVehicleModelInMessage()
    {
        // Arrange
        var vehicleModel = "Honda Civic";

        // Act
        var exception = new VehicleNotAvailableException(vehicleModel);

        // Assert
        exception.Message.Should().Contain(vehicleModel);
    }

    [Fact]
    public void Exception_ShouldBeThrowable()
    {
        // Arrange
        var vehicleModel = "Ford Focus";
        Action act = () => throw new VehicleNotAvailableException(vehicleModel);

        // Act & Assert
        act.Should().Throw<VehicleNotAvailableException>()
            .WithMessage("*Ford Focus*");
    }

    [Fact]
    public void Exception_ShouldBeDomainException()
    {
        // Act
        var exception = new VehicleNotAvailableException("Toyota Corolla");

        // Assert
        exception.Should().BeAssignableTo<VehicleCRM.Domain.Common.Exceptions.DomainException>();
    }

    [Fact]
    public void Constructor_WithDifferentVehicleModels_ShouldCreateExceptionWithCorrectMessage()
    {
        // Arrange
        var model1 = "Chevrolet Onix";
        var model2 = "Volkswagen Gol";
        var model3 = "Fiat Uno";

        // Act
        var exception1 = new VehicleNotAvailableException(model1);
        var exception2 = new VehicleNotAvailableException(model2);
        var exception3 = new VehicleNotAvailableException(model3);

        // Assert
        exception1.Message.Should().Contain(model1);
        exception2.Message.Should().Contain(model2);
        exception3.Message.Should().Contain(model3);
    }
}
