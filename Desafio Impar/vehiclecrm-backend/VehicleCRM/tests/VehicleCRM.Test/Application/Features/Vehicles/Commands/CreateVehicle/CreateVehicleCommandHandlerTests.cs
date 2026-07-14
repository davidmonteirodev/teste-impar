using VehicleCRM.Application.Common.Models;
using VehicleCRM.Application.Features.Vehicles.Commands;
using VehicleCRM.Domain.Vehicles.Entities;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Test.Application.Features.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandHandlerTests
{
    private readonly IVehicleRepository _repositoryMock;
    private readonly CreateVehicleCommandHandler _handler;

    public CreateVehicleCommandHandlerTests()
    {
        _repositoryMock = Substitute.For<IVehicleRepository>();
        _handler = new CreateVehicleCommandHandler(_repositoryMock);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateVehicleAndReturnCreatedResponse()
    {
        // Arrange
        var command = new CreateVehicleCommand(
            Brand: "Toyota",
            Model: "Corolla",
            Year: 2023,
            Price: 85000m,
            Color: "Branco",
            Mileage: 15000
        );

        _repositoryMock
            .InsertAsync(Arg.Any<Vehicle>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask)
            .AndDoes(callInfo =>
            {
                var vehicle = callInfo.Arg<Vehicle>();
                typeof(Vehicle).GetProperty(nameof(Vehicle.Id))!.SetValue(vehicle, 1L);
                typeof(Vehicle).GetProperty(nameof(Vehicle.CreateDate))!.SetValue(vehicle, DateTime.UtcNow);
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<EntityCreatedResponse>();
        result.Id.Should().BeGreaterThan(0);
        result.CreateDate.Should().NotBe(default);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryInsertWithCorrectVehicleData()
    {
        // Arrange
        var command = new CreateVehicleCommand(
            Brand: "Honda",
            Model: "Civic",
            Year: 2024,
            Price: 95000m,
            Color: "Preto",
            Mileage: 5000
        );

        Vehicle? capturedVehicle = null;
        _repositoryMock
            .InsertAsync(Arg.Do<Vehicle>(vehicle => capturedVehicle = vehicle), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedVehicle.Should().NotBeNull();
        capturedVehicle!.Brand.Should().Be(command.Brand);
        capturedVehicle.Model.Should().Be(command.Model);
        capturedVehicle.Year.Should().Be(command.Year);
        capturedVehicle.Price.Should().Be(command.Price);
        capturedVehicle.Color.Should().Be(command.Color);
        capturedVehicle.Mileage.Should().Be(command.Mileage);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryInsertOnce()
    {
        // Arrange
        var command = new CreateVehicleCommand("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        _repositoryMock
            .InsertAsync(Arg.Any<Vehicle>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _repositoryMock.Received(1).InsertAsync(Arg.Any<Vehicle>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var command = new CreateVehicleCommand("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        var cancellationToken = new CancellationToken();

        _repositoryMock
            .InsertAsync(Arg.Any<Vehicle>(), cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        await _repositoryMock.Received(1).InsertAsync(Arg.Any<Vehicle>(), cancellationToken);
    }
}
