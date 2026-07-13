using VehicleCRM.Domain.Vehicles.Exceptions;

namespace VehicleCRM.Test.Domain.Vehicles.Exceptions;

public class VehicleHasSaleOpportunitiesExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetCorrectMessage()
    {
        // Act
        var exception = new VehicleHasSaleOpportunitiesException();

        // Assert
        exception.Message.Should().Be("Não é possível excluir um veículo que possui oportunidades de venda associadas.");
    }

    [Fact]
    public void Exception_ShouldBeThrowable()
    {
        // Arrange
        Action act = () => throw new VehicleHasSaleOpportunitiesException();

        // Act & Assert
        act.Should().Throw<VehicleHasSaleOpportunitiesException>()
            .WithMessage("Não é possível excluir um veículo que possui oportunidades de venda associadas.");
    }

    [Fact]
    public void Exception_ShouldBeDomainException()
    {
        // Act
        var exception = new VehicleHasSaleOpportunitiesException();

        // Assert
        exception.Should().BeAssignableTo<VehicleCRM.Domain.Common.Exceptions.DomainException>();
    }
}
