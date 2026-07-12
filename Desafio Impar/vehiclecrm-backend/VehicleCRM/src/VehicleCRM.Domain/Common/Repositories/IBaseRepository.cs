using VehicleCRM.Domain.Common.Entities;

namespace VehicleCRM.Domain.Common.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task InsertAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
