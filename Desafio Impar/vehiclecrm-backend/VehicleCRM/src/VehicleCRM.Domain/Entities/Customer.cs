using VehicleCRM.Domain.Entities.Base;
using VehicleCRM.Domain.Enums;

namespace VehicleCRM.Domain.Entities
{
    public class Customer : BaseEntity
    {
        protected Customer() { }

        public Customer(string name, string email, string phone, CustomerMainInterest mainInterest)
        {
            Name = name;
            Email = email;
            Phone = phone;
            MainInterest = mainInterest;
        }

        public virtual string Name { get; private set; }
        public virtual string Email { get; private set; }
        public virtual string Phone { get; private set; }
        public virtual CustomerMainInterest MainInterest { get; private set; }

        public virtual void Update(string name, string email, string phone, CustomerMainInterest mainInterest)
        {
            Name = name;
            Email = email;
            Phone = phone;
            MainInterest = mainInterest;
        }
    }
}
