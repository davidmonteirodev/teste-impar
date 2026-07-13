using VehicleCRM.Domain.Customers.ValueObjects;

namespace VehicleCRM.Test.Domain.Customers.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Create_WithValidEmail_ShouldCreateEmailValueObject()
    {
        // Arrange
        var emailString = "test@example.com";

        // Act
        var email = Email.Create(emailString);

        // Assert
        email.Should().NotBeNull();
        email.Value.Should().Be("test@example.com");
    }

    [Fact]
    public void Create_WithUppercaseEmail_ShouldNormalizeToLowercase()
    {
        // Arrange
        var emailString = "TEST@EXAMPLE.COM";

        // Act
        var email = Email.Create(emailString);

        // Assert
        email.Value.Should().Be("test@example.com");
    }

    [Fact]
    public void Create_WithEmailWithSpaces_ShouldTrimSpaces()
    {
        // Arrange
        var emailString = "  test@example.com  ";

        // Act
        var email = Email.Create(emailString);

        // Assert
        email.Value.Should().Be("test@example.com");
    }

    [Fact]
    public void Create_WithNullEmail_ShouldReturnEmptyString()
    {
        // Arrange
        string? emailString = null;

        // Act
        var email = Email.Create(emailString);

        // Assert
        email.Value.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithEmptyEmail_ShouldReturnEmptyString()
    {
        // Arrange
        var emailString = "";

        // Act
        var email = Email.Create(emailString);

        // Assert
        email.Value.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithWhitespaceEmail_ShouldReturnEmptyString()
    {
        // Arrange
        var emailString = "   ";

        // Act
        var email = Email.Create(emailString);

        // Assert
        email.Value.Should().BeEmpty();
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var email1 = Email.Create("test@example.com");
        var email2 = Email.Create("test@example.com");

        // Act & Assert
        email1.Should().Be(email2);
    }

    [Fact]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var email1 = Email.Create("test1@example.com");
        var email2 = Email.Create("test2@example.com");

        // Act & Assert
        email1.Should().NotBe(email2);
    }

    [Fact]
    public void ToString_ShouldReturnEmailValue()
    {
        // Arrange
        var email = Email.Create("test@example.com");

        // Act
        var result = email.ToString();

        // Assert
        result.Should().Be("test@example.com");
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldWork()
    {
        // Arrange
        var email = Email.Create("test@example.com");

        // Act
        string emailString = email;

        // Assert
        emailString.Should().Be("test@example.com");
    }
}
