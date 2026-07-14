using VehicleCRM.Application.Common.Exceptions;
using VehicleCRM.Application.Features.SaleOpportunities.Commands;
using VehicleCRM.Domain.Common.Exceptions;
using VehicleCRM.Domain.Common.UnitOfWork;
using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.Customers.Enums;
using VehicleCRM.Domain.Customers.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Entities;
using VehicleCRM.Domain.SaleOpportunities.Enums;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.Vehicles.Entities;
using VehicleCRM.Domain.Vehicles.Enums;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Test.Application.Features.SaleOpportunities.Commands.UpdateSaleOpportunity;

public class UpdateSaleOpportunityCommandHandlerTests
{
    private readonly ISaleOpportunityRepository _saleOpportunityRepositoryMock;
    private readonly ICustomerRepository _customerRepositoryMock;
    private readonly IVehicleRepository _vehicleRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateSaleOpportunityCommandHandler _handler;

    public UpdateSaleOpportunityCommandHandlerTests()
    {
        _saleOpportunityRepositoryMock = Substitute.For<ISaleOpportunityRepository>();
        _customerRepositoryMock = Substitute.For<ICustomerRepository>();
        _vehicleRepositoryMock = Substitute.For<IVehicleRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _handler = new UpdateSaleOpportunityCommandHandler(
            _saleOpportunityRepositoryMock,
            _customerRepositoryMock,
            _vehicleRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldUpdateSaleOpportunity()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas originais");

        var command = new UpdateSaleOpportunityCommand(
            Id: 1,
            CustomerId: 1,
            VehicleId: 1,
            Status: SaleOpportunityStatus.InNegotiation,
            ProposedValue: 82000m,
            Notes: "Notas atualizadas"
        );

        SetupSuccessfulScenario(saleOpportunity, customer, vehicle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        saleOpportunity.Status.Should().Be(SaleOpportunityStatus.InNegotiation);
        saleOpportunity.ProposedValue.Should().Be(82000m);
        saleOpportunity.Notes.Should().Be("Notas atualizadas");
    }

    [Fact]
    public async Task Handle_WhenSaleOpportunityNotFound_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var command = new UpdateSaleOpportunityCommand(1, 1, 1, SaleOpportunityStatus.InNegotiation, 80000m, "Notas");

        _saleOpportunityRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((SaleOpportunity?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage("*Oportunidade de Venda*1*");
    }

    [Fact]
    public async Task Handle_WhenCustomerNotFound_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");
        var command = new UpdateSaleOpportunityCommand(1, 1, 1, SaleOpportunityStatus.InNegotiation, 80000m, "Notas");

        _saleOpportunityRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(saleOpportunity);

        _customerRepositoryMock
            .GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns((Customer?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage("*Cliente*1*");
    }

    [Fact]
    public async Task Handle_WhenVehicleNotFound_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");
        var customer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var command = new UpdateSaleOpportunityCommand(1, 1, 1, SaleOpportunityStatus.InNegotiation, 80000m, "Notas");

        _saleOpportunityRepositoryMock.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(saleOpportunity);
        _customerRepositoryMock.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>()).Returns(customer);
        _vehicleRepositoryMock.GetByIdAsync(command.VehicleId, Arg.Any<CancellationToken>()).Returns((Vehicle?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage("*Veículo*1*");
    }

    [Fact]
    public async Task Handle_ShouldBeginAndCommitTransaction()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");
        var command = new UpdateSaleOpportunityCommand(1, 1, 1, SaleOpportunityStatus.InNegotiation, 82000m, "Notas atualizadas");

        SetupSuccessfulScenario(saleOpportunity, customer, vehicle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWorkMock.Received(1).BeginTransactionAsync(Arg.Any<CancellationToken>());
        await _unitOfWorkMock.Received(1).CommitTransactionAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenVehicleChanged_ShouldCheckForActiveOpportunity()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var oldVehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        var newVehicle = Vehicle.Create("Honda", "Civic", 2024, 95000m, "Preto", 5000);
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");
        var command = new UpdateSaleOpportunityCommand(1, 1, 2, SaleOpportunityStatus.InNegotiation, 90000m, "Notas");

        _saleOpportunityRepositoryMock.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(saleOpportunity);
        _customerRepositoryMock.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>()).Returns(customer);
        _vehicleRepositoryMock.GetByIdAsync(1L, Arg.Any<CancellationToken>()).Returns(oldVehicle);
        _vehicleRepositoryMock.GetByIdAsync(2L, Arg.Any<CancellationToken>()).Returns(newVehicle);
        _saleOpportunityRepositoryMock.HasActiveOpportunityForVehicleAsync(2L, command.Id, Arg.Any<CancellationToken>()).Returns(false);
        _unitOfWorkMock.BeginTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _unitOfWorkMock.CommitTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _saleOpportunityRepositoryMock.UpdateAsync(Arg.Any<SaleOpportunity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleOpportunityRepositoryMock.Received(1)
            .HasActiveOpportunityForVehicleAsync(2L, command.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenVehicleChangedAndHasActiveOpportunity_ShouldThrowDomainException()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");
        var command = new UpdateSaleOpportunityCommand(1, 1, 2, SaleOpportunityStatus.InNegotiation, 90000m, "Notas");

        _saleOpportunityRepositoryMock.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(saleOpportunity);
        _customerRepositoryMock.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>()).Returns(customer);
        _vehicleRepositoryMock.GetByIdAsync(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns(vehicle);
        _saleOpportunityRepositoryMock.HasActiveOpportunityForVehicleAsync(2L, command.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*já existe uma oportunidade ativa*");
    }

    [Fact]
    public async Task Handle_WhenVehicleChanged_ShouldSetPreviousVehicleAsAvailable()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var oldVehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        oldVehicle.UpdateStatus(VehicleSaleStatus.Reserved);
        var newVehicle = Vehicle.Create("Honda", "Civic", 2024, 95000m, "Preto", 5000);
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");
        var command = new UpdateSaleOpportunityCommand(1, 1, 2, SaleOpportunityStatus.InNegotiation, 90000m, "Notas");

        _saleOpportunityRepositoryMock.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(saleOpportunity);
        _customerRepositoryMock.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>()).Returns(customer);
        _vehicleRepositoryMock.GetByIdAsync(1L, Arg.Any<CancellationToken>()).Returns(oldVehicle);
        _vehicleRepositoryMock.GetByIdAsync(2L, Arg.Any<CancellationToken>()).Returns(newVehicle);
        _saleOpportunityRepositoryMock.HasActiveOpportunityForVehicleAsync(2L, command.Id, Arg.Any<CancellationToken>()).Returns(false);
        _unitOfWorkMock.BeginTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _unitOfWorkMock.CommitTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _saleOpportunityRepositoryMock.UpdateAsync(Arg.Any<SaleOpportunity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        oldVehicle.Status.Should().Be(VehicleSaleStatus.Available);
    }

    [Fact]
    public async Task Handle_WhenStatusChangedToInNegotiation_ShouldReserveVehicle()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");
        var command = new UpdateSaleOpportunityCommand(1, 1, 1, SaleOpportunityStatus.InNegotiation, 82000m, "Notas");

        SetupSuccessfulScenario(saleOpportunity, customer, vehicle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        vehicle.Status.Should().Be(VehicleSaleStatus.Reserved);
    }

    [Fact]
    public async Task Handle_ShouldCallUpdateAsyncOnce()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        var saleOpportunity = SaleOpportunity.Create(1, 1, 80000m, "Notas");
        var command = new UpdateSaleOpportunityCommand(1, 1, 1, SaleOpportunityStatus.InNegotiation, 82000m, "Notas");

        SetupSuccessfulScenario(saleOpportunity, customer, vehicle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleOpportunityRepositoryMock.Received(1).UpdateAsync(saleOpportunity, Arg.Any<CancellationToken>());
    }

    private void SetupSuccessfulScenario(SaleOpportunity saleOpportunity, Customer customer, Vehicle vehicle)
    {
        _saleOpportunityRepositoryMock.GetByIdAsync(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns(saleOpportunity);
        _customerRepositoryMock.GetByIdAsync(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns(customer);
        _vehicleRepositoryMock.GetByIdAsync(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns(vehicle);
        _saleOpportunityRepositoryMock.HasActiveOpportunityForVehicleAsync(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns(false);
        _unitOfWorkMock.BeginTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _unitOfWorkMock.CommitTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _saleOpportunityRepositoryMock.UpdateAsync(Arg.Any<SaleOpportunity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
    }
}
