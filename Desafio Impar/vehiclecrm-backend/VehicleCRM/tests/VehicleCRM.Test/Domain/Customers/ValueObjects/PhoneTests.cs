using VehicleCRM.Domain.Customers.ValueObjects;

namespace VehicleCRM.Test.Domain.Customers.ValueObjects;

public class PhoneTests
{
    [Fact]
    public void Create_WithValidPhone_ShouldCreatePhoneValueObject()
    {
        // Arrange
        var phoneString = "11987654321";

        // Act
        var phone = Phone.Create(phoneString);

        // Assert
        phone.Should().NotBeNull();
        phone.Value.Should().Be("11987654321");
    }

    [Fact]
    public void Create_WithPhoneWithFormatting_ShouldRemoveNonNumericCharacters()
    {
        // Arrange
        var phoneString = "(11) 98765-4321";

        // Act
        var phone = Phone.Create(phoneString);

        // Assert
        phone.Value.Should().Be("11987654321");
    }

    [Fact]
    public void Create_WithPhoneWithSpaces_ShouldRemoveSpaces()
    {
        // Arrange
        var phoneString = "11 98765 4321";

        // Act
        var phone = Phone.Create(phoneString);

        // Assert
        phone.Value.Should().Be("11987654321");
    }

    [Fact]
    public void Create_WithPhoneWithSpecialCharacters_ShouldRemoveSpecialCharacters()
    {
        // Arrange
        var phoneString = "+55 (11) 98765-4321";

        // Act
        var phone = Phone.Create(phoneString);

        // Assert
        phone.Value.Should().Be("5511987654321");
    }

    [Fact]
    public void Create_WithNullPhone_ShouldReturnEmptyString()
    {
        // Arrange
        string? phoneString = null;

        // Act
        var phone = Phone.Create(phoneString);

        // Assert
        phone.Value.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithEmptyPhone_ShouldReturnEmptyString()
    {
        // Arrange
        var phoneString = "";

        // Act
        var phone = Phone.Create(phoneString);

        // Assert
        phone.Value.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithWhitespacePhone_ShouldReturnEmptyString()
    {
        // Arrange
        var phoneString = "   ";

        // Act
        var phone = Phone.Create(phoneString);

        // Assert
        phone.Value.Should().BeEmpty();
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var phone1 = Phone.Create("11987654321");
        var phone2 = Phone.Create("11987654321");

        // Act & Assert
        phone1.Should().Be(phone2);
    }

    [Fact]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var phone1 = Phone.Create("11987654321");
        var phone2 = Phone.Create("11912345678");

        // Act & Assert
        phone1.Should().NotBe(phone2);
    }

    [Fact]
    public void ToString_ShouldReturnPhoneValue()
    {
        // Arrange
        var phone = Phone.Create("11987654321");

        // Act
        var result = phone.ToString();

        // Assert
        result.Should().Be("11987654321");
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldWork()
    {
        // Arrange
        var phone = Phone.Create("11987654321");

        // Act
        string phoneString = phone;

        // Assert
        phoneString.Should().Be("11987654321");
    }
}
