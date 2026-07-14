using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.Customers.Enums;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Services;
using VehicleCRM.Domain.Vehicles.Entities;
using VehicleCRM.Domain.Vehicles.Enums;
using VehicleCRM.Domain.Vehicles.Exceptions;
using VehicleCRM.Domain.SaleOpportunities.Exceptions;

namespace VehicleCRM.Test.Domain.SaleOpportunities.Services;

public class SaleOpportunityDomainServiceTests
{
    private readonly ISaleOpportunityRepository _repositoryMock;
    private readonly SaleOpportunityDomainService _service;

    public SaleOpportunityDomainServiceTests()
    {
        _repositoryMock = Substitute.For<ISaleOpportunityRepository>();
        _service = new SaleOpportunityDomainService(_repositoryMock);
    }

    [Fact]
    public async Task ValidateNewSaleOpportunityAsync_WhenVehicleIsAvailable_AndNoExistingOpportunity_ShouldNotThrowException()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "11987654321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        _repositoryMock
            .HasActiveOpportunityForVehicleAsync(Arg.Any<long>(), Arg.Any<long?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        _repositoryMock
            .ExistsByCustomerAndVehicleAsync(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        Func<Task> act = async () => await _service.ValidateNewSaleOpportunityAsync(customer, vehicle);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ValidateNewSaleOpportunityAsync_WhenVehicleIsNotAvailable_ShouldThrowVehicleNotAvailableException()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "11987654321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        vehicle.UpdateStatus(VehicleSaleStatus.Sold);

        // Act
        Func<Task> act = async () => await _service.ValidateNewSaleOpportunityAsync(customer, vehicle);

        // Assert
        await act.Should().ThrowAsync<VehicleNotAvailableException>()
            .WithMessage("*Corolla*");
    }

    [Fact]
    public async Task ValidateNewSaleOpportunityAsync_WhenVehicleIsReserved_ShouldThrowVehicleNotAvailableException()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "11987654321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        vehicle.UpdateStatus(VehicleSaleStatus.Reserved);

        // Act
        Func<Task> act = async () => await _service.ValidateNewSaleOpportunityAsync(customer, vehicle);

        // Assert
        await act.Should().ThrowAsync<VehicleNotAvailableException>();
    }

    [Fact]
    public async Task ValidateNewSaleOpportunityAsync_WhenOpportunityAlreadyExists_ShouldThrowDuplicateSaleOpportunityException()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "11987654321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        _repositoryMock
            .HasActiveOpportunityForVehicleAsync(Arg.Any<long>(), Arg.Any<long?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        _repositoryMock
            .ExistsByCustomerAndVehicleAsync(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        Func<Task> act = async () => await _service.ValidateNewSaleOpportunityAsync(customer, vehicle);

        // Assert
        await act.Should().ThrowAsync<DuplicateSaleOpportunityException>()
            .WithMessage("*João Silva*Corolla*");
    }

    [Fact]
    public async Task ValidateNewSaleOpportunityAsync_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "11987654321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        _repositoryMock
            .HasActiveOpportunityForVehicleAsync(Arg.Any<long>(), Arg.Any<long?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        _repositoryMock
            .ExistsByCustomerAndVehicleAsync(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        await _service.ValidateNewSaleOpportunityAsync(customer, vehicle);

        // Assert
        await _repositoryMock.Received(1).HasActiveOpportunityForVehicleAsync(vehicle.Id, null, Arg.Any<CancellationToken>());
        await _repositoryMock.Received(1).ExistsByCustomerAndVehicleAsync(customer.Id, vehicle.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ValidateNewSaleOpportunityAsync_WithCancellationToken_ShouldPassCancellationTokenToRepository()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "11987654321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        var cancellationToken = new CancellationToken();

        _repositoryMock
            .HasActiveOpportunityForVehicleAsync(Arg.Any<long>(), Arg.Any<long?>(), cancellationToken)
            .Returns(false);

        _repositoryMock
            .ExistsByCustomerAndVehicleAsync(Arg.Any<long>(), Arg.Any<long>(), cancellationToken)
            .Returns(false);

        // Act
        await _service.ValidateNewSaleOpportunityAsync(customer, vehicle, cancellationToken);

        // Assert
        await _repositoryMock.Received(1).HasActiveOpportunityForVehicleAsync(vehicle.Id, null, cancellationToken);
        await _repositoryMock.Received(1).ExistsByCustomerAndVehicleAsync(customer.Id, vehicle.Id, cancellationToken);
    }

    [Fact]
    public async Task ValidateNewSaleOpportunityAsync_ShouldCheckVehicleStatusBeforeCheckingRepository()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "11987654321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        vehicle.UpdateStatus(VehicleSaleStatus.Sold);

        // Act
        Func<Task> act = async () => await _service.ValidateNewSaleOpportunityAsync(customer, vehicle);

        // Assert
        await act.Should().ThrowAsync<VehicleNotAvailableException>();
        await _repositoryMock.DidNotReceive().HasActiveOpportunityForVehicleAsync(Arg.Any<long>(), Arg.Any<long?>(), Arg.Any<CancellationToken>());
        await _repositoryMock.DidNotReceive().ExistsByCustomerAndVehicleAsync(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ValidateNewSaleOpportunityAsync_WhenVehicleHasActiveOpportunity_ShouldThrowVehicleInActiveOpportunityException()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "11987654321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        _repositoryMock
            .HasActiveOpportunityForVehicleAsync(Arg.Any<long>(), Arg.Any<long?>(), Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        Func<Task> act = async () => await _service.ValidateNewSaleOpportunityAsync(customer, vehicle);

        // Assert
        await act.Should().ThrowAsync<VehicleInActiveOpportunityException>()
            .WithMessage("*Corolla*");
        await _repositoryMock.DidNotReceive().ExistsByCustomerAndVehicleAsync(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ValidateNewSaleOpportunityAsync_WhenVehicleHasOnlyLostOpportunities_ShouldNotThrowException()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "11987654321", CustomerMainInterest.Sedan);
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        _repositoryMock
            .HasActiveOpportunityForVehicleAsync(Arg.Any<long>(), Arg.Any<long?>(), Arg.Any<CancellationToken>())
            .Returns(false);

        _repositoryMock
            .ExistsByCustomerAndVehicleAsync(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        Func<Task> act = async () => await _service.ValidateNewSaleOpportunityAsync(customer, vehicle);

        // Assert
        await act.Should().NotThrowAsync();
    }
}
