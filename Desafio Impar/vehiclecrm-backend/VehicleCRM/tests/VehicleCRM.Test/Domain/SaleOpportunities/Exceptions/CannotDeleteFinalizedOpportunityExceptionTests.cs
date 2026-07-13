using VehicleCRM.Domain.SaleOpportunities.Exceptions;

namespace VehicleCRM.Test.Domain.SaleOpportunities.Exceptions;

public class CannotDeleteFinalizedOpportunityExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetCorrectMessage()
    {
        // Act
        var exception = new CannotDeleteFinalizedOpportunityException();

        // Assert
        exception.Message.Should().Be("Não é permitido excluir uma oportunidade de venda finalizada (com status 'Vendido' ou 'Perdido').");
    }

    [Fact]
    public void Exception_ShouldBeThrowable()
    {
        // Arrange
        Action act = () => throw new CannotDeleteFinalizedOpportunityException();

        // Act & Assert
        act.Should().Throw<CannotDeleteFinalizedOpportunityException>()
            .WithMessage("Não é permitido excluir uma oportunidade de venda finalizada (com status 'Vendido' ou 'Perdido').");
    }

    [Fact]
    public void Exception_ShouldBeDomainException()
    {
        // Act
        var exception = new CannotDeleteFinalizedOpportunityException();

        // Assert
        exception.Should().BeAssignableTo<VehicleCRM.Domain.Common.Exceptions.DomainException>();
    }
}
