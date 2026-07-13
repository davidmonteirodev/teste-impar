using VehicleCRM.Application.Common.Models;
using VehicleCRM.Application.Features.Customers.Commands;
using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.Customers.Enums;
using VehicleCRM.Domain.Customers.Exceptions;
using VehicleCRM.Domain.Customers.Repositories;

namespace VehicleCRM.Test.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandlerTests
{
    private readonly ICustomerRepository _repositoryMock;
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerCommandHandlerTests()
    {
        _repositoryMock = Substitute.For<ICustomerRepository>();
        _handler = new CreateCustomerCommandHandler(_repositoryMock);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateCustomerAndReturnCreatedResponse()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            Name: "João Silva",
            Email: "joao@example.com",
            Phone: "(11) 98765-4321",
            MainInterest: CustomerMainInterest.Sedan
        );

        _repositoryMock
            .ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(false);

        _repositoryMock
            .InsertAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask)
            .AndDoes(callInfo =>
            {
                var customer = callInfo.Arg<Customer>();
                typeof(Customer).GetProperty(nameof(Customer.Id))!.SetValue(customer, 1L);
                typeof(Customer).GetProperty(nameof(Customer.CreateDate))!.SetValue(customer, DateTime.UtcNow);
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
    public async Task Handle_WhenEmailAlreadyExists_ShouldThrowDuplicateCustomerEmailException()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            "João Silva",
            "joao@example.com",
            "11987654321",
            CustomerMainInterest.Sedan
        );

        _repositoryMock
            .ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DuplicateCustomerEmailException>()
            .WithMessage("*joao@example.com*");
    }

    [Fact]
    public async Task Handle_ShouldCheckEmailExistenceBeforeInserting()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            "João Silva",
            "joao@example.com",
            "11987654321",
            CustomerMainInterest.Sedan
        );

        _repositoryMock
            .ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(false);

        _repositoryMock
            .InsertAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _repositoryMock.Received(1).ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenEmailExists_ShouldNotCallInsertAsync()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            "João Silva",
            "joao@example.com",
            "11987654321",
            CustomerMainInterest.Sedan
        );

        _repositoryMock
            .ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        try
        {
            await _handler.Handle(command, CancellationToken.None);
        }
        catch (DuplicateCustomerEmailException)
        {
            // Expected exception
        }

        // Assert
        await _repositoryMock.DidNotReceive().InsertAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryInsertWithCorrectCustomerData()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            "Maria Santos",
            "maria@example.com",
            "+55 (11) 91234-5678",
            CustomerMainInterest.Suv
        );

        Customer? capturedCustomer = null;
        _repositoryMock
            .ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(false);

        _repositoryMock
            .InsertAsync(Arg.Do<Customer>(c => capturedCustomer = c), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedCustomer.Should().NotBeNull();
        capturedCustomer!.Name.Should().Be(command.Name);
        capturedCustomer.Email.Value.Should().Be("maria@example.com");
        capturedCustomer.Phone.Value.Should().Be("5511912345678");
        capturedCustomer.MainInterest.Should().Be(command.MainInterest);
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToRepositoryMethods()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            "João Silva",
            "joao@example.com",
            "11987654321",
            CustomerMainInterest.Sedan
        );
        var cancellationToken = new CancellationToken();

        _repositoryMock
            .ExistsByEmailAsync(command.Email, cancellationToken)
            .Returns(false);

        _repositoryMock
            .InsertAsync(Arg.Any<Customer>(), cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        await _repositoryMock.Received(1).ExistsByEmailAsync(command.Email, cancellationToken);
        await _repositoryMock.Received(1).InsertAsync(Arg.Any<Customer>(), cancellationToken);
    }
}
