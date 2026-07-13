using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.Customers.Enums;
using VehicleCRM.Domain.Customers.ValueObjects;

namespace VehicleCRM.Test.Domain.Customers.Entities;


public class CustomerTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateCustomer()
    {
        // Arrange
        var name = "João Silva";
        var email = "joao@example.com";
        var phone = "(11) 98765-4321";
        var mainInterest = CustomerMainInterest.Sedan;

        // Act
        var customer = Customer.Create(name, email, phone, mainInterest);

        // Assert
        customer.Should().NotBeNull();
        customer.Name.Should().Be(name);
        customer.Email.Value.Should().Be("joao@example.com");
        customer.Phone.Value.Should().Be("11987654321");
        customer.MainInterest.Should().Be(mainInterest);
    }

    [Fact]
    public void Create_WithDifferentInterests_ShouldCreateCustomerWithCorrectInterest()
    {
        // Arrange
        var name = "Maria Santos";
        var email = "maria@example.com";
        var phone = "11987654321";

        // Act
        var customerSuv = Customer.Create(name, email, phone, CustomerMainInterest.Suv);
        var customerHatch = Customer.Create(name, email, phone, CustomerMainInterest.Hatch);
        var customerUtility = Customer.Create(name, email, phone, CustomerMainInterest.Utility);

        // Assert
        customerSuv.MainInterest.Should().Be(CustomerMainInterest.Suv);
        customerHatch.MainInterest.Should().Be(CustomerMainInterest.Hatch);
        customerUtility.MainInterest.Should().Be(CustomerMainInterest.Utility);
    }

    [Fact]
    public void Create_WithEmailInUppercase_ShouldNormalizeEmail()
    {
        // Arrange
        var email = "JOAO@EXAMPLE.COM";

        // Act
        var customer = Customer.Create("João Silva", email, "11987654321", CustomerMainInterest.Sedan);

        // Assert
        customer.Email.Value.Should().Be("joao@example.com");
    }

    [Fact]
    public void Create_WithFormattedPhone_ShouldNormalizePhone()
    {
        // Arrange
        var phone = "+55 (11) 98765-4321";

        // Act
        var customer = Customer.Create("João Silva", "joao@example.com", phone, CustomerMainInterest.Sedan);

        // Assert
        customer.Phone.Value.Should().Be("5511987654321");
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateCustomerProperties()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "11987654321", CustomerMainInterest.Sedan);
        var newName = "João Silva Santos";
        var newPhone = "(11) 91234-5678";
        var newInterest = CustomerMainInterest.Suv;

        // Act
        customer.Update(newName, newPhone, newInterest);

        // Assert
        customer.Name.Should().Be(newName);
        customer.Phone.Value.Should().Be("11912345678");
        customer.MainInterest.Should().Be(newInterest);
    }

    [Fact]
    public void Update_ShouldNotChangeEmail()
    {
        // Arrange
        var originalEmail = "joao@example.com";
        var customer = Customer.Create("João Silva", originalEmail, "11987654321", CustomerMainInterest.Sedan);

        // Act
        customer.Update("João Silva Santos", "11912345678", CustomerMainInterest.Suv);

        // Assert
        customer.Email.Value.Should().Be(originalEmail);
    }

    [Fact]
    public void Update_WithFormattedPhone_ShouldNormalizePhone()
    {
        // Arrange
        var customer = Customer.Create("João Silva", "joao@example.com", "11987654321", CustomerMainInterest.Sedan);
        var newPhone = "+55 (11) 91234-5678";

        // Act
        customer.Update("João Silva", newPhone, CustomerMainInterest.Sedan);

        // Assert
        customer.Phone.Value.Should().Be("5511912345678");
    }

    [Fact]
    public void Create_WithAllInterestTypes_ShouldCreateCorrectly()
    {
        // Arrange & Act & Assert
        var suv = Customer.Create("Test", "test@test.com", "11987654321", CustomerMainInterest.Suv);
        suv.MainInterest.Should().Be(CustomerMainInterest.Suv);

        var hatch = Customer.Create("Test", "test@test.com", "11987654321", CustomerMainInterest.Hatch);
        hatch.MainInterest.Should().Be(CustomerMainInterest.Hatch);

        var sedan = Customer.Create("Test", "test@test.com", "11987654321", CustomerMainInterest.Sedan);
        sedan.MainInterest.Should().Be(CustomerMainInterest.Sedan);

        var utility = Customer.Create("Test", "test@test.com", "11987654321", CustomerMainInterest.Utility);
        utility.MainInterest.Should().Be(CustomerMainInterest.Utility);

        var usedCar = Customer.Create("Test", "test@test.com", "11987654321", CustomerMainInterest.UsedCar);
        usedCar.MainInterest.Should().Be(CustomerMainInterest.UsedCar);

        var newCar = Customer.Create("Test", "test@test.com", "11987654321", CustomerMainInterest.NewCar);
        newCar.MainInterest.Should().Be(CustomerMainInterest.NewCar);
    }
}

