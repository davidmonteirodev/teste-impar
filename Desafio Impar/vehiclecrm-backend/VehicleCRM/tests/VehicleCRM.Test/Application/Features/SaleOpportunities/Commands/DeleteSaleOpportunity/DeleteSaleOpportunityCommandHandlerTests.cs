using VehicleCRM.Application.Common.Exceptions;
using VehicleCRM.Application.Features.SaleOpportunities.Commands;
using VehicleCRM.Domain.Common.UnitOfWork;
using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.Customers.Enums;
using VehicleCRM.Domain.SaleOpportunities.Entities;
using VehicleCRM.Domain.SaleOpportunities.Enums;
using VehicleCRM.Domain.SaleOpportunities.Exceptions;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.Vehicles.Entities;
using VehicleCRM.Domain.Vehicles.Enums;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Test.Application.Features.SaleOpportunities.Commands.DeleteSaleOpportunity;

public class DeleteSaleOpportunityCommandHandlerTests
{
    private readonly ISaleOpportunityRepository _saleOpportunityRepositoryMock;
    private readonly IVehicleRepository _vehicleRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly DeleteSaleOpportunityCommandHandler _handler;

    public DeleteSaleOpportunityCommandHandlerTests()
    {
        _saleOpportunityRepositoryMock = Substitute.For<ISaleOpportunityRepository>();
        _vehicleRepositoryMock = Substitute.For<IVehicleRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _handler = new DeleteSaleOpportunityCommandHandler(
            _saleOpportunityRepositoryMock,
            _vehicleRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_WhenSaleOpportunityNotFound_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var command = new DeleteSaleOpportunityCommand(1);

        _saleOpportunityRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((SaleOpportunity?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage("*Oportunidade de venda*1*");
    }

    [Fact]
    public async Task Handle_WhenSaleOpportunityIsFinalized_ShouldThrowCannotDeleteFinalizedOpportunityException()
    {
        // Arrange
        var command = new DeleteSaleOpportunityCommand(1);
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");
        saleOpportunity.Update(1, 1, SaleOpportunityStatus.Sold, 80000m, "Notas");

        _saleOpportunityRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(saleOpportunity);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CannotDeleteFinalizedOpportunityException>();
    }

    [Fact]
    public async Task Handle_WithValidNonFinalizedOpportunity_ShouldDeleteOpportunity()
    {
        // Arrange
        var command = new DeleteSaleOpportunityCommand(1);
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        _saleOpportunityRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(saleOpportunity);

        _vehicleRepositoryMock
            .GetByIdAsync(saleOpportunity.VehicleId, Arg.Any<CancellationToken>())
            .Returns(vehicle);

        _unitOfWorkMock
            .BeginTransactionAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .CommitTransactionAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _saleOpportunityRepositoryMock
            .DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleOpportunityRepositoryMock.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldBeginAndCommitTransaction()
    {
        // Arrange
        var command = new DeleteSaleOpportunityCommand(1);
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        SetupSuccessfulScenario(saleOpportunity, vehicle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWorkMock.Received(1).BeginTransactionAsync(Arg.Any<CancellationToken>());
        await _unitOfWorkMock.Received(1).CommitTransactionAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenVehicleExists_ShouldSetVehicleAsAvailable()
    {
        // Arrange
        var command = new DeleteSaleOpportunityCommand(1);
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        vehicle.UpdateStatus(VehicleSaleStatus.Reserved);

        SetupSuccessfulScenario(saleOpportunity, vehicle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        vehicle.Status.Should().Be(VehicleSaleStatus.Available);
        await _vehicleRepositoryMock.Received(1).UpdateAsync(vehicle, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenVehicleNotFound_ShouldStillDeleteOpportunity()
    {
        // Arrange
        var command = new DeleteSaleOpportunityCommand(1);
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");

        _saleOpportunityRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(saleOpportunity);

        _vehicleRepositoryMock
            .GetByIdAsync(saleOpportunity.VehicleId, Arg.Any<CancellationToken>())
            .Returns((Vehicle?)null);

        _unitOfWorkMock.BeginTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _unitOfWorkMock.CommitTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _saleOpportunityRepositoryMock.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleOpportunityRepositoryMock.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
        await _vehicleRepositoryMock.DidNotReceive().UpdateAsync(Arg.Any<Vehicle>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenVehicleAlreadyAvailable_ShouldNotUpdateVehicle()
    {
        // Arrange
        var command = new DeleteSaleOpportunityCommand(1);
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        SetupSuccessfulScenario(saleOpportunity, vehicle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        vehicle.Status.Should().Be(VehicleSaleStatus.Available);
        await _vehicleRepositoryMock.DidNotReceive().UpdateAsync(vehicle, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenFinalized_ShouldNotCallDeleteAsync()
    {
        // Arrange
        var command = new DeleteSaleOpportunityCommand(1);
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");
        saleOpportunity.Update(1, 1, SaleOpportunityStatus.Lost, 80000m, "Notas");

        _saleOpportunityRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(saleOpportunity);

        // Act
        try
        {
            await _handler.Handle(command, CancellationToken.None);
        }
        catch (CannotDeleteFinalizedOpportunityException)
        {
            // Expected exception
        }

        // Assert
        await _saleOpportunityRepositoryMock.DidNotReceive().DeleteAsync(Arg.Any<long>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassItToAllAsync()
    {
        // Arrange
        var command = new DeleteSaleOpportunityCommand(1);
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        _saleOpportunityRepositoryMock.GetByIdAsync(command.Id, token).Returns(saleOpportunity);
        _vehicleRepositoryMock.GetByIdAsync(saleOpportunity.VehicleId, token).Returns(vehicle);
        _unitOfWorkMock.BeginTransactionAsync(token).Returns(Task.CompletedTask);
        _unitOfWorkMock.CommitTransactionAsync(token).Returns(Task.CompletedTask);
        _saleOpportunityRepositoryMock.DeleteAsync(command.Id, token).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, token);

        // Assert
        await _saleOpportunityRepositoryMock.Received(1).GetByIdAsync(command.Id, token);
        await _vehicleRepositoryMock.Received(1).GetByIdAsync(saleOpportunity.VehicleId, token);
        await _unitOfWorkMock.Received(1).BeginTransactionAsync(token);
        await _unitOfWorkMock.Received(1).CommitTransactionAsync(token);
        await _saleOpportunityRepositoryMock.Received(1).DeleteAsync(command.Id, token);
    }

    private void SetupSuccessfulScenario(SaleOpportunity saleOpportunity, Vehicle vehicle)
    {
        _saleOpportunityRepositoryMock.GetByIdAsync(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns(saleOpportunity);
        _vehicleRepositoryMock.GetByIdAsync(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns(vehicle);
        _unitOfWorkMock.BeginTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _unitOfWorkMock.CommitTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _saleOpportunityRepositoryMock.DeleteAsync(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
    }
}
