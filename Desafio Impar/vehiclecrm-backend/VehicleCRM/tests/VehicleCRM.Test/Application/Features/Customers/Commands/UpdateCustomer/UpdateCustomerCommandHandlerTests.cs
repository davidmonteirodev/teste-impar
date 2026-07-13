using VehicleCRM.Application.Common.Exceptions;
using VehicleCRM.Application.Features.Customers.Commands;
using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.Customers.Enums;
using VehicleCRM.Domain.Customers.Repositories;

namespace VehicleCRM.Test.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandlerTests
{
    private readonly ICustomerRepository _repositoryMock;
    private readonly UpdateCustomerCommandHandler _handler;

    public UpdateCustomerCommandHandlerTests()
    {
        _repositoryMock = Substitute.For<ICustomerRepository>();
        _handler = new UpdateCustomerCommandHandler(_repositoryMock);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldUpdateCustomer()
    {
        // Arrange
        var existingCustomer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var command = new UpdateCustomerCommand(
            Id: 1,
            Name: "João Silva Santos",
            Phone: "(11) 91234-5678",
            MainInterest: CustomerMainInterest.Suv
        );

        _repositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingCustomer);

        _repositoryMock
            .UpdateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        existingCustomer.Name.Should().Be(command.Name);
        existingCustomer.Phone.Value.Should().Be("11912345678");
        existingCustomer.MainInterest.Should().Be(command.MainInterest);
    }

    [Fact]
    public async Task Handle_WhenCustomerNotFound_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var command = new UpdateCustomerCommand(1, "João Silva", "(11) 98765-4321", CustomerMainInterest.Sedan);

        _repositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Customer?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage("*Cliente*1*");
    }

    [Fact]
    public async Task Handle_ShouldCallGetByIdFirst()
    {
        // Arrange
        var existingCustomer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var command = new UpdateCustomerCommand(1, "João Silva Santos", "(11) 91234-5678", CustomerMainInterest.Suv);

        _repositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingCustomer);

        _repositoryMock
            .UpdateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
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
        var existingCustomer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var command = new UpdateCustomerCommand(1, "João Silva Santos", "(11) 91234-5678", CustomerMainInterest.Suv);

        _repositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingCustomer);

        _repositoryMock
            .UpdateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _repositoryMock.Received(1).UpdateAsync(existingCustomer, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCustomerNotFound_ShouldNotCallUpdateAsync()
    {
        // Arrange
        var command = new UpdateCustomerCommand(1, "João Silva", "(11) 98765-4321", CustomerMainInterest.Sedan);

        _repositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Customer?)null);

        // Act
        try
        {
            await _handler.Handle(command, CancellationToken.None);
        }
        catch (EntityNotFoundException)
        {
            // Expected exception
        }

        // Assert
        await _repositoryMock.DidNotReceive().UpdateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldUpdateAllProperties()
    {
        // Arrange
        var existingCustomer = Customer.Create("Nome Original", "original@example.com", "(11) 99999-9999", CustomerMainInterest.Hatch);
        var command = new UpdateCustomerCommand(1, "Nome Atualizado", "(11) 88888-8888", CustomerMainInterest.Utility);

        _repositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingCustomer);

        _repositoryMock
            .UpdateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        existingCustomer.Name.Should().Be("Nome Atualizado");
        existingCustomer.Phone.Value.Should().Be("11888888888");
        existingCustomer.MainInterest.Should().Be(CustomerMainInterest.Utility);
    }

    [Fact]
    public async Task Handle_WhenCancellationRequested_ShouldPassCancellationToken()
    {
        // Arrange
        var existingCustomer = Customer.Create("João Silva", "joao@example.com", "(11) 98765-4321", CustomerMainInterest.Sedan);
        var command = new UpdateCustomerCommand(1, "João Silva Santos", "(11) 91234-5678", CustomerMainInterest.Suv);
        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        _repositoryMock
            .GetByIdAsync(command.Id, token)
            .Returns(existingCustomer);

        _repositoryMock
            .UpdateAsync(existingCustomer, token)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, token);

        // Assert
        await _repositoryMock.Received(1).GetByIdAsync(command.Id, token);
        await _repositoryMock.Received(1).UpdateAsync(existingCustomer, token);
    }
}
