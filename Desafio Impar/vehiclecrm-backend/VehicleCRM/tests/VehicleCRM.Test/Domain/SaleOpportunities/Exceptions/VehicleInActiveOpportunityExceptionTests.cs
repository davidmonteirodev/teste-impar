using VehicleCRM.Domain.Common.Exceptions;
using VehicleCRM.Domain.SaleOpportunities.Exceptions;

namespace VehicleCRM.Test.Domain.SaleOpportunities.Exceptions;

public class VehicleInActiveOpportunityExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetCorrectMessage_WithVehicleModel()
    {
        // Arrange
        var vehicleModel = "Toyota Corolla";

        // Act
        var exception = new VehicleInActiveOpportunityException(vehicleModel);

        // Assert
        exception.Message.Should().Be("O veículo 'Toyota Corolla' já está em outra oportunidade de venda ativa.");
    }

    [Fact]
    public void Constructor_ShouldBeOfTypeDomainException()
    {
        // Arrange
        var vehicleModel = "Honda Civic";

        // Act
        var exception = new VehicleInActiveOpportunityException(vehicleModel);

        // Assert
        exception.Should().BeAssignableTo<DomainException>();
    }
}
