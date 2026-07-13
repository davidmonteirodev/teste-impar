using VehicleCRM.Application.Common.Exceptions;
using VehicleCRM.Application.Features.Vehicles.Commands;
using VehicleCRM.Domain.Vehicles.Entities;
using VehicleCRM.Domain.Vehicles.Enums;
using VehicleCRM.Domain.Vehicles.Exceptions;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Test.Application.Features.Vehicles.Commands.UpdateVehicle;

public class UpdateVehicleCommandHandlerTests
{
    private readonly IVehicleRepository _repositoryMock;
    private readonly UpdateVehicleCommandHandler _handler;

    public UpdateVehicleCommandHandlerTests()
    {
        _repositoryMock = Substitute.For<IVehicleRepository>();
        _handler = new UpdateVehicleCommandHandler(_repositoryMock);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldUpdateVehicle()
    {
        // Arrange
        var existingVehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        var command = new UpdateVehicleCommand(
            Id: 1,
            Brand: "Honda",
            Model: "Civic",
            Year: 2024,
            Price: 95000m,
            Color: "Preto",
            Mileage: 5000
        );

        _repositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingVehicle);

        _repositoryMock
            .UpdateAsync(Arg.Any<Vehicle>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        existingVehicle.Brand.Should().Be(command.Brand);
        existingVehicle.Model.Should().Be(command.Model);
        existingVehicle.Year.Should().Be(command.Year);
        existingVehicle.Price.Should().Be(command.Price);
        existingVehicle.Color.Should().Be(command.Color);
        existingVehicle.Mileage.Should().Be(command.Mileage);
    }

    [Fact]
    public async Task Handle_WhenVehicleNotFound_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var command = new UpdateVehicleCommand(1, "Honda", "Civic", 2024, 95000m, "Preto", 5000);

        _repositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Vehicle?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage("*Veículo*1*");
    }

    [Fact]
    public async Task Handle_ShouldCallGetByIdFirst()
    {
        // Arrange
        var existingVehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        var command = new UpdateVehicleCommand(1, "Honda", "Civic", 2024, 95000m, "Preto", 5000);

        _repositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingVehicle);

        _repositoryMock
            .UpdateAsync(Arg.Any<Vehicle>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _repositoryMock.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldCallUpdateAsyncOnce()
    {
        // Arrange
        var existingVehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        var command = new UpdateVehicleCommand(1, "Honda", "Civic", 2024, 95000m, "Preto", 5000);

        _repositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingVehicle);

        _repositoryMock
            .UpdateAsync(Arg.Any<Vehicle>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _repositoryMock.Received(1).UpdateAsync(existingVehicle, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenVehicleIsSold_ShouldThrowVehicleCannotBeEditedException()
    {
        // Arrange
        var existingVehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        existingVehicle.UpdateStatus(VehicleSaleStatus.Sold);
        var command = new UpdateVehicleCommand(1, "Honda", "Civic", 2024, 95000m, "Preto", 5000);

        _repositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingVehicle);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<VehicleCannotBeEditedException>();
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToRepositoryMethods()
    {
        // Arrange
        var existingVehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        var command = new UpdateVehicleCommand(1, "Honda", "Civic", 2024, 95000m, "Preto", 5000);
        var cancellationToken = new CancellationToken();

        _repositoryMock
            .GetByIdAsync(command.Id, cancellationToken)
            .Returns(existingVehicle);

        _repositoryMock
            .UpdateAsync(Arg.Any<Vehicle>(), cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        await _repositoryMock.Received(1).GetByIdAsync(command.Id, cancellationToken);
        await _repositoryMock.Received(1).UpdateAsync(existingVehicle, cancellationToken);
    }
}
