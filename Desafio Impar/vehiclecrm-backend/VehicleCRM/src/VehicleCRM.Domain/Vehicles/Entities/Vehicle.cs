using VehicleCRM.Domain.Common.Entities;
using VehicleCRM.Domain.Vehicles.Enums;

namespace VehicleCRM.Domain.Vehicles.Entities
{
    public class Vehicle : BaseEntity
    {
        protected Vehicle() { }

        public Vehicle(string brand, string model, int year, decimal price, string color, int mileage, VehicleSaleStatus status)
        {
            Brand = brand;
            Model = model;
            Year = year;
            Price = price;
            Color = color;
            Mileage = mileage;
            Status = status;
        }

        public virtual string Brand { get; private set; }
        public virtual string Model { get; private set; }
        public virtual int Year { get; private set; }
        public virtual decimal Price { get; private set; }
        public virtual string Color { get; private set; }
        public virtual int Mileage { get; private set; }
        public virtual VehicleSaleStatus Status { get; private set; }

        public virtual void Update(string brand, string model, int year, decimal price, string color, int mileage, VehicleSaleStatus status)
        {
            Brand = brand;
            Model = model;
            Year = year;
            Price = price;
            Color = color;
            Mileage = mileage;
            Status = status;
        }
    }
}
