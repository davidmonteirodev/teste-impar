using VehicleCRM.Domain.Common.Entities;
using VehicleCRM.Domain.Customers.Enums;
using VehicleCRM.Domain.Customers.ValueObjects;

namespace VehicleCRM.Domain.Customers.Entities
{
    public class Customer : BaseEntity
    {
        protected Customer() { }

        public Customer(string name, string email, string phone, CustomerMainInterest mainInterest)
        {
            Name = name;
            Email = email;
            Phone = Phone.Create(phone);
            MainInterest = mainInterest;
        }

        public virtual string Name { get; private set; }
        public virtual string Email { get; private set; }
        public virtual Phone Phone { get; private set; }
        public virtual CustomerMainInterest MainInterest { get; private set; }

        public virtual void Update(string name, string email, string phone, CustomerMainInterest mainInterest)
        {
            Name = name;
            Email = email;
            Phone = Phone.Create(phone);
            MainInterest = mainInterest;
        }
    }
}
