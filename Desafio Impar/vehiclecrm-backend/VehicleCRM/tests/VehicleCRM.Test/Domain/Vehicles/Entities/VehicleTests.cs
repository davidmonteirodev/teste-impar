using VehicleCRM.Domain.Vehicles.Entities;
using VehicleCRM.Domain.Vehicles.Enums;
using VehicleCRM.Domain.Vehicles.Exceptions;

namespace VehicleCRM.Test.Domain.Vehicles.Entities;

public class VehicleTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateVehicleWithAvailableStatus()
    {
        // Arrange
        var brand = "Toyota";
        var model = "Corolla";
        var year = 2023;
        var price = 85000m;
        var color = "Branco";
        var mileage = 15000;

        // Act
        var vehicle = Vehicle.Create(brand, model, year, price, color, mileage);

        // Assert
        vehicle.Should().NotBeNull();
        vehicle.Brand.Should().Be(brand);
        vehicle.Model.Should().Be(model);
        vehicle.Year.Should().Be(year);
        vehicle.Price.Should().Be(price);
        vehicle.Color.Should().Be(color);
        vehicle.Mileage.Should().Be(mileage);
        vehicle.Status.Should().Be(VehicleSaleStatus.Available);
    }

    [Fact]
    public void Update_WhenVehicleIsAvailable_ShouldUpdateAllProperties()
    {
        // Arrange
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        var newBrand = "Honda";
        var newModel = "Civic";
        var newYear = 2024;
        var newPrice = 95000m;
        var newColor = "Preto";
        var newMileage = 10000;

        // Act
        vehicle.Update(newBrand, newModel, newYear, newPrice, newColor, newMileage);

        // Assert
        vehicle.Brand.Should().Be(newBrand);
        vehicle.Model.Should().Be(newModel);
        vehicle.Year.Should().Be(newYear);
        vehicle.Price.Should().Be(newPrice);
        vehicle.Color.Should().Be(newColor);
        vehicle.Mileage.Should().Be(newMileage);
    }

    [Fact]
    public void Update_WhenVehicleIsSold_ShouldThrowVehicleCannotBeEditedException()
    {
        // Arrange
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        vehicle.UpdateStatus(VehicleSaleStatus.Sold);

        // Act
        Action act = () => vehicle.Update("Honda", "Civic", 2024, 95000m, "Preto", 10000);

        // Assert
        act.Should().Throw<VehicleCannotBeEditedException>()
            .WithMessage("*vendido*");
    }

    [Fact]
    public void Update_WhenVehicleIsReserved_ShouldUpdateSuccessfully()
    {
        // Arrange
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        vehicle.UpdateStatus(VehicleSaleStatus.Reserved);
        var newBrand = "Honda";

        // Act
        vehicle.Update(newBrand, "Civic", 2024, 95000m, "Preto", 10000);

        // Assert
        vehicle.Brand.Should().Be(newBrand);
    }

    [Fact]
    public void UpdateStatus_WithValidStatus_ShouldUpdateStatus()
    {
        // Arrange
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        // Act
        vehicle.UpdateStatus(VehicleSaleStatus.Reserved);

        // Assert
        vehicle.Status.Should().Be(VehicleSaleStatus.Reserved);
    }

    [Fact]
    public void UpdateStatus_ToSold_ShouldUpdateToSold()
    {
        // Arrange
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

        // Act
        vehicle.UpdateStatus(VehicleSaleStatus.Sold);

        // Assert
        vehicle.Status.Should().Be(VehicleSaleStatus.Sold);
    }

    [Fact]
    public void UpdateStatus_FromSoldToAvailable_ShouldAllowStatusChange()
    {
        // Arrange
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);
        vehicle.UpdateStatus(VehicleSaleStatus.Sold);

        // Act
        vehicle.UpdateStatus(VehicleSaleStatus.Available);

        // Assert
        vehicle.Status.Should().Be(VehicleSaleStatus.Available);
    }

    [Fact]
    public void Create_WithZeroPrice_ShouldCreateVehicle()
    {
        // Arrange & Act
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 0m, "Branco", 15000);

        // Assert
        vehicle.Price.Should().Be(0m);
    }

    [Fact]
    public void Create_WithZeroMileage_ShouldCreateVehicle()
    {
        // Arrange & Act
        var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 0);

        // Assert
        vehicle.Mileage.Should().Be(0);
    }
}
