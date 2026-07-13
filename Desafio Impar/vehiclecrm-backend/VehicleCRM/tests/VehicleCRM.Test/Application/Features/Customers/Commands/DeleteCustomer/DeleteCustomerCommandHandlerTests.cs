using VehicleCRM.Application.Features.Customers.Commands;
using VehicleCRM.Domain.Customers.Exceptions;
using VehicleCRM.Domain.Customers.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Repositories;

namespace VehicleCRM.Test.Application.Features.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandHandlerTests
{
    private readonly ICustomerRepository _customerRepositoryMock;
    private readonly ISaleOpportunityRepository _saleOpportunityRepositoryMock;
    private readonly DeleteCustomerCommandHandler _handler;

    public DeleteCustomerCommandHandlerTests()
    {
        _customerRepositoryMock = Substitute.For<ICustomerRepository>();
        _saleOpportunityRepositoryMock = Substitute.For<ISaleOpportunityRepository>();
        _handler = new DeleteCustomerCommandHandler(
            _customerRepositoryMock,
            _saleOpportunityRepositoryMock);
    }

    [Fact]
    public async Task Handle_WhenCustomerHasNoOpportunities_ShouldDeleteCustomer()
    {
        // Arrange
        var command = new DeleteCustomerCommand(1);

        _saleOpportunityRepositoryMock
            .HasAnyOpportunityForCustomerAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        _customerRepositoryMock
            .DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _customerRepositoryMock.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCustomerHasOpportunities_ShouldThrowCustomerHasSaleOpportunitiesException()
    {
        // Arrange
        var command = new DeleteCustomerCommand(1);

        _saleOpportunityRepositoryMock
            .HasAnyOpportunityForCustomerAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CustomerHasSaleOpportunitiesException>();
    }

    [Fact]
    public async Task Handle_WhenCustomerHasOpportunities_ShouldNotCallDeleteAsync()
    {
        // Arrange
        var command = new DeleteCustomerCommand(1);

        _saleOpportunityRepositoryMock
            .HasAnyOpportunityForCustomerAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        try
        {
            await _handler.Handle(command, CancellationToken.None);
        }
        catch (CustomerHasSaleOpportunitiesException)
        {
            // Expected exception
        }

        // Assert
        await _customerRepositoryMock.DidNotReceive().DeleteAsync(Arg.Any<long>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldCheckOpportunitiesBeforeDeleting()
    {
        // Arrange
        var command = new DeleteCustomerCommand(1);

        _saleOpportunityRepositoryMock
            .HasAnyOpportunityForCustomerAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        _customerRepositoryMock
            .DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleOpportunityRepositoryMock.Received(1)
            .HasAnyOpportunityForCustomerAsync(command.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldPassCorrectCustomerId()
    {
        // Arrange
        var customerId = 123L;
        var command = new DeleteCustomerCommand(customerId);

        _saleOpportunityRepositoryMock
            .HasAnyOpportunityForCustomerAsync(customerId, Arg.Any<CancellationToken>())
            .Returns(false);

        _customerRepositoryMock
            .DeleteAsync(customerId, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _customerRepositoryMock.Received(1).DeleteAsync(customerId, Arg.Any<CancellationToken>());
        await _saleOpportunityRepositoryMock.Received(1).HasAnyOpportunityForCustomerAsync(customerId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCancellationRequested_ShouldPassCancellationToken()
    {
        // Arrange
        var command = new DeleteCustomerCommand(1);
        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        _saleOpportunityRepositoryMock
            .HasAnyOpportunityForCustomerAsync(command.Id, token)
            .Returns(false);

        _customerRepositoryMock
            .DeleteAsync(command.Id, token)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, token);

        // Assert
        await _saleOpportunityRepositoryMock.Received(1).HasAnyOpportunityForCustomerAsync(command.Id, token);
        await _customerRepositoryMock.Received(1).DeleteAsync(command.Id, token);
    }
}
