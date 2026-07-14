using VehicleCRM.Domain.Customers.Exceptions;

namespace VehicleCRM.Test.Domain.Customers.Exceptions;

public class CustomerHasSaleOpportunitiesExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetCorrectMessage()
    {
        // Act
        var exception = new CustomerHasSaleOpportunitiesException();

        // Assert
        exception.Message.Should().Be("Não é possível excluir um cliente que possui oportunidades de venda associadas.");
    }

    [Fact]
    public void Exception_ShouldBeThrowable()
    {
        // Arrange
        Action act = () => throw new CustomerHasSaleOpportunitiesException();

        // Act & Assert
        act.Should().Throw<CustomerHasSaleOpportunitiesException>()
            .WithMessage("Não é possível excluir um cliente que possui oportunidades de venda associadas.");
    }

    [Fact]
    public void Exception_ShouldBeDomainException()
    {
        // Act
        var exception = new CustomerHasSaleOpportunitiesException();

        // Assert
        exception.Should().BeAssignableTo<VehicleCRM.Domain.Common.Exceptions.DomainException>();
    }
}
