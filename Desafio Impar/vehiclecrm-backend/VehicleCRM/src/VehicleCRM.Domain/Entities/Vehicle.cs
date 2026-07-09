using VehicleCRM.Domain.Entities.Base;
using VehicleCRM.Domain.Enums;

namespace VehicleCRM.Domain.Entities
{
    public class Vehicle : BaseEntity
    {
        protected Vehicle() { }

        public Vehicle(int brandId, string model, int year, decimal price, string color, int mileage, VehicleSaleStatus status)
        {
            BrandId = brandId;
            Model = model;
            Year = year;
            Price = price;
            Color = color;
            Mileage = mileage;
            Status = status;
        }

        public virtual int BrandId { get; private set; }
        public virtual string Model { get; private set; }
        public virtual int Year { get; private set; }
        public virtual decimal Price { get; private set; }
        public virtual string Color { get; private set; }
        public virtual int Mileage { get; private set; }
        public virtual VehicleSaleStatus Status { get; private set; }
        public Brand Brand { get; protected set; }
    }
}