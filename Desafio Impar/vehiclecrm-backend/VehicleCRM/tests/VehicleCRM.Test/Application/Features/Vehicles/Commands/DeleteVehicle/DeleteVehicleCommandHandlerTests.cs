using VehicleCRM.Application.Features.Vehicles.Commands;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.Vehicles.Exceptions;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Test.Application.Features.Vehicles.Commands.DeleteVehicle;

public class DeleteVehicleCommandHandlerTests
{
    private readonly IVehicleRepository _vehicleRepositoryMock;
    private readonly ISaleOpportunityRepository _saleOpportunityRepositoryMock;
    private readonly DeleteVehicleCommandHandler _handler;

    public DeleteVehicleCommandHandlerTests()
    {
        _vehicleRepositoryMock = Substitute.For<IVehicleRepository>();
        _saleOpportunityRepositoryMock = Substitute.For<ISaleOpportunityRepository>();
        _handler = new DeleteVehicleCommandHandler(
            _vehicleRepositoryMock,
            _saleOpportunityRepositoryMock);
    }

    [Fact]
    public async Task Handle_WhenVehicleHasNoOpportunities_ShouldDeleteVehicle()
    {
        // Arrange
        var command = new DeleteVehicleCommand(1);

        _saleOpportunityRepositoryMock
            .HasAnyOpportunityForVehicleAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        _vehicleRepositoryMock
            .DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _vehicleRepositoryMock.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenVehicleHasOpportunities_ShouldThrowVehicleHasSaleOpportunitiesException()
    {
        // Arrange
        var command = new DeleteVehicleCommand(1);

        _saleOpportunityRepositoryMock
            .HasAnyOpportunityForVehicleAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<VehicleHasSaleOpportunitiesException>();
    }

    [Fact]
    public async Task Handle_WhenVehicleHasOpportunities_ShouldNotCallDeleteAsync()
    {
        // Arrange
        var command = new DeleteVehicleCommand(1);

        _saleOpportunityRepositoryMock
            .HasAnyOpportunityForVehicleAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        try
        {
            await _handler.Handle(command, CancellationToken.None);
        }
        catch (VehicleHasSaleOpportunitiesException)
        {
            // Expected exception
        }

        // Assert
        await _vehicleRepositoryMock.DidNotReceive().DeleteAsync(Arg.Any<long>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldCheckOpportunitiesBeforeDeleting()
    {
        // Arrange
        var command = new DeleteVehicleCommand(1);

        _saleOpportunityRepositoryMock
            .HasAnyOpportunityForVehicleAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        _vehicleRepositoryMock
            .DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleOpportunityRepositoryMock.Received(1).HasAnyOpportunityForVehicleAsync(command.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToRepositoryMethods()
    {
        // Arrange
        var command = new DeleteVehicleCommand(1);
        var cancellationToken = new CancellationToken();

        _saleOpportunityRepositoryMock
            .HasAnyOpportunityForVehicleAsync(command.Id, cancellationToken)
            .Returns(false);

        _vehicleRepositoryMock
            .DeleteAsync(command.Id, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        await _saleOpportunityRepositoryMock.Received(1).HasAnyOpportunityForVehicleAsync(command.Id, cancellationToken);
        await _vehicleRepositoryMock.Received(1).DeleteAsync(command.Id, cancellationToken);
    }

    [Fact]
    public async Task Handle_WithValidId_ShouldPassCorrectIdToRepositories()
    {
        // Arrange
        var vehicleId = 42L;
        var command = new DeleteVehicleCommand(vehicleId);

        _saleOpportunityRepositoryMock
            .HasAnyOpportunityForVehicleAsync(vehicleId, Arg.Any<CancellationToken>())
            .Returns(false);

        _vehicleRepositoryMock
            .DeleteAsync(vehicleId, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleOpportunityRepositoryMock.Received(1).HasAnyOpportunityForVehicleAsync(vehicleId, Arg.Any<CancellationToken>());
        await _vehicleRepositoryMock.Received(1).DeleteAsync(vehicleId, Arg.Any<CancellationToken>());
    }
}
