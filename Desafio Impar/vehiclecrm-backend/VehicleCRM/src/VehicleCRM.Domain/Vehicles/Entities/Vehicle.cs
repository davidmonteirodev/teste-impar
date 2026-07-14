using VehicleCRM.Domain.Common.Entities;
using VehicleCRM.Domain.Vehicles.Enums;
using VehicleCRM.Domain.Vehicles.Exceptions;

namespace VehicleCRM.Domain.Vehicles.Entities
{
    public class Vehicle : BaseEntity
    {
        protected Vehicle() { }

        private Vehicle(string brand, string model, int year, decimal price, string color, int mileage, VehicleSaleStatus status)
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

        public static Vehicle Create(string brand, string model, int year, decimal price, string color, int mileage)
        {
            return new Vehicle(brand, model, year, price, color, mileage, VehicleSaleStatus.Available);
        }

        public virtual void Update(string brand, string model, int year, decimal price, string color, int mileage)
        {
            if (Status == VehicleSaleStatus.Sold)
            {
                throw new VehicleCannotBeEditedException("vendido");
            }

            Brand = brand;
            Model = model;
            Year = year;
            Price = price;
            Color = color;
            Mileage = mileage;
        }

        public virtual void UpdateStatus(VehicleSaleStatus status)
        {
            Status = status;
        }
    }
}
