using VehicleCRM.Domain.Entities.Base;

namespace VehicleCRM.Domain.Entities
{
    public class Brand : BaseEntity
    {
        protected Brand() { }
        
        public Brand(string? name)
        {
            Name = name;
        }

        public virtual string? Name { get; protected set; }
    }
}