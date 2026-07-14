namespace VehicleCRM.Domain.Common.Entities
{
    public abstract class BaseEntity
    {
        public virtual long Id { get; protected set; }
        public virtual DateTime CreateDate { get; protected set; }
        public virtual DateTime? ModificationDate { get; protected set; }
        public virtual DateTime? DeleteDate { get; protected set; }
        public virtual bool IsDeleted { get; protected set; }
    }
}
