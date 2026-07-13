using VehicleCRM.Application.Common.Exceptions;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Application.Features.SaleOpportunities.Commands;
using VehicleCRM.Domain.Common.UnitOfWork;
using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.Customers.Enums;
using VehicleCRM.Domain.Customers.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Entities;
using VehicleCRM.Domain.SaleOpportunities.Enums;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Services;
using VehicleCRM.Domain.Vehicles.Entities;
using VehicleCRM.Domain.Vehicles.Enums;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Test.Application.Features.SaleOpportunities.Commands.CreateSaleOpportunity;

public class CreateSaleOpportunityCommandHandlerTests
{
    private readonly ISaleOpportunityRepository _saleOpportunityRepositoryMock;
    private readonly ICustomerRepository _customerRepositoryMock;
    private readonly IVehicleRepository _vehicleRepositoryMock;
    private readonly ISaleOpportunityDomainService _saleOpportunityDomainServiceMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateSaleOpportunityCommandHandler _handler;

    public CreateSaleOpportunityCommandHandlerTests()
    {
        _saleOpportunityRepositoryMock = Substitute.For<ISaleOpportunityRepository>();
        _customerRepositoryMock = Substitute.For<ICustomerRepository>();
        _vehicleRepositoryMock = Substitute.For<IVehicleRepository>();
        _saleOpportunityDomainServiceMock = Substitute.For<ISaleOpportunityDomainService>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _handler = new CreateSaleOpportunityCommandHandler(
            _saleOpportunityRepositoryMock,
            _customerRepositoryMock,
            _vehicleRepositoryMock,
            _saleOpportunityDomainServiceMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateSaleOpportunityAndReturnCreatedResponse()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(
            CustomerId: 1,
            VehicleId: 1,
            ProposedValue: 80000m,
            Notes: "Cliente interessado"
        );

        var customer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        _customerRepositoryMock
            .GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(customer);

        _vehicleRepositoryMock
            .GetByIdAsync(command.VehicleId, Arg.Any<CancellationToken>())
            .Returns(vehicle);

        _saleOpportunityDomainServiceMock
            .ValidateNewSaleOpportunityAsync(customer, vehicle, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .BeginTransactionAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .CommitTransactionAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _saleOpportunityRepositoryMock
            .InsertAsync(Arg.Any<SaleOpportunity>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask)
            .AndDoes(callInfo =>
            {
                var opportunity = callInfo.Arg<SaleOpportunity>();
                typeof(SaleOpportunity).GetProperty(nameof(SaleOpportunity.Id))!.SetValue(opportunity, 1L);
                typeof(SaleOpportunity).GetProperty(nameof(SaleOpportunity.CreateDate))!.SetValue(opportunity, DateTime.UtcNow);
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
    public async Task Handle_WhenCustomerNotFound_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(1, 1, 80000m, "Cliente interessado");

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
        var command = new CreateSaleOpportunityCommand(1, 1, 80000m, "Cliente interessado");
        var customer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);

        _customerRepositoryMock
            .GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(customer);

        _vehicleRepositoryMock
            .GetByIdAsync(command.VehicleId, Arg.Any<CancellationToken>())
            .Returns((Vehicle?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage("*Veículo*1*");
    }

    [Fact]
    public async Task Handle_ShouldCallDomainServiceToValidateNewSaleOpportunity()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(1, 1, 80000m, "Cliente interessado");
        var customer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        _customerRepositoryMock.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>()).Returns(customer);
        _vehicleRepositoryMock.GetByIdAsync(command.VehicleId, Arg.Any<CancellationToken>()).Returns(vehicle);
        _saleOpportunityDomainServiceMock.ValidateNewSaleOpportunityAsync(customer, vehicle, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _unitOfWorkMock.BeginTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _unitOfWorkMock.CommitTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _saleOpportunityRepositoryMock.InsertAsync(Arg.Any<SaleOpportunity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleOpportunityDomainServiceMock.Received(1)
            .ValidateNewSaleOpportunityAsync(customer, vehicle, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldCallInsertAsyncOnce()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(1, 1, 80000m, "Cliente interessado");
        var customer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        SetupSuccessfulScenario(customer, vehicle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleOpportunityRepositoryMock.Received(1).InsertAsync(Arg.Any<SaleOpportunity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldBeginAndCommitTransaction()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(1, 1, 80000m, "Cliente interessado");
        var customer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        SetupSuccessfulScenario(customer, vehicle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWorkMock.Received(1).BeginTransactionAsync(Arg.Any<CancellationToken>());
        await _unitOfWorkMock.Received(1).CommitTransactionAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldNotUpdateVehicleStatusForNewLead()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(1, 1, 80000m, "Cliente interessado");
        var customer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        SetupSuccessfulScenario(customer, vehicle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        vehicle.Status.Should().Be(VehicleSaleStatus.Available);
        await _vehicleRepositoryMock.DidNotReceive().UpdateAsync(vehicle, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldCreateSaleOpportunityWithCorrectData()
    {
        // Arrange
        var command = new CreateSaleOpportunityCommand(1, 2, 95000m, "Observações importantes");
        var customer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        typeof(Customer).GetProperty(nameof(Customer.Id))!.SetValue(customer, 1L);

        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        typeof(Vehicle).GetProperty(nameof(Vehicle.Id))!.SetValue(vehicle, 2L);

        _customerRepositoryMock.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>()).Returns(customer);
        _vehicleRepositoryMock.GetByIdAsync(command.VehicleId, Arg.Any<CancellationToken>()).Returns(vehicle);
        _saleOpportunityDomainServiceMock.ValidateNewSaleOpportunityAsync(customer, vehicle, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _unitOfWorkMock.BeginTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _unitOfWorkMock.CommitTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        SaleOpportunity? capturedOpportunity = null;
        _saleOpportunityRepositoryMock
            .InsertAsync(Arg.Do<SaleOpportunity>(opp => capturedOpportunity = opp), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedOpportunity.Should().NotBeNull();
        capturedOpportunity!.CustomerId.Should().Be(1L);
        capturedOpportunity.VehicleId.Should().Be(2L);
        capturedOpportunity.ProposedValue.Should().Be(command.ProposedValue);
        capturedOpportunity.Notes.Should().Be(command.Notes);
        capturedOpportunity.Status.Should().Be(SaleOpportunityStatus.NewLead);
    }

    private void SetupSuccessfulScenario(Customer customer, Vehicle vehicle)
    {
        _customerRepositoryMock.GetByIdAsync(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns(customer);
        _vehicleRepositoryMock.GetByIdAsync(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns(vehicle);
        _saleOpportunityDomainServiceMock.ValidateNewSaleOpportunityAsync(customer, vehicle, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _unitOfWorkMock.BeginTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _unitOfWorkMock.CommitTransactionAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _saleOpportunityRepositoryMock.InsertAsync(Arg.Any<SaleOpportunity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
    }
}
