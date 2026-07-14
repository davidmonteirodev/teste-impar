using VehicleCRM.Domain.SaleOpportunities.Exceptions;

namespace VehicleCRM.Test.Domain.SaleOpportunities.Exceptions;

public class DuplicateSaleOpportunityExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetCorrectMessage()
    {
        // Arrange
        var customerName = "João Silva";
        var vehicleModel = "Toyota Corolla";

        // Act
        var exception = new DuplicateSaleOpportunityException(customerName, vehicleModel);

        // Assert
        exception.Message.Should().Be("Já existe uma oportunidade de venda para o cliente 'João Silva' e veículo 'Toyota Corolla'.");
    }

    [Fact]
    public void Constructor_ShouldIncludeCustomerNameInMessage()
    {
        // Arrange
        var customerName = "Maria Santos";
        var vehicleModel = "Honda Civic";

        // Act
        var exception = new DuplicateSaleOpportunityException(customerName, vehicleModel);

        // Assert
        exception.Message.Should().Contain(customerName);
    }

    [Fact]
    public void Constructor_ShouldIncludeVehicleModelInMessage()
    {
        // Arrange
        var customerName = "Pedro Oliveira";
        var vehicleModel = "Ford Focus";

        // Act
        var exception = new DuplicateSaleOpportunityException(customerName, vehicleModel);

        // Assert
        exception.Message.Should().Contain(vehicleModel);
    }

    [Fact]
    public void Exception_ShouldBeThrowable()
    {
        // Arrange
        var customerName = "João Silva";
        var vehicleModel = "Toyota Corolla";
        Action act = () => throw new DuplicateSaleOpportunityException(customerName, vehicleModel);

        // Act & Assert
        act.Should().Throw<DuplicateSaleOpportunityException>()
            .WithMessage("*João Silva*Toyota Corolla*");
    }

    [Fact]
    public void Exception_ShouldBeDomainException()
    {
        // Act
        var exception = new DuplicateSaleOpportunityException("Cliente", "Veículo");

        // Assert
        exception.Should().BeAssignableTo<VehicleCRM.Domain.Common.Exceptions.DomainException>();
    }

    [Fact]
    public void Constructor_WithEmptyStrings_ShouldStillCreateException()
    {
        // Arrange
        var customerName = "";
        var vehicleModel = "";

        // Act
        var exception = new DuplicateSaleOpportunityException(customerName, vehicleModel);

        // Assert
        exception.Message.Should().Be("Já existe uma oportunidade de venda para o cliente '' e veículo ''.");
    }
}
