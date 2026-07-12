using Microsoft.EntityFrameworkCore;
using VehicleCRM.Domain.Common.Entities;
using VehicleCRM.Domain.Common.Repositories;
using VehicleCRM.Infrastructure.Persistence.Contexts;

namespace VehicleCRM.Infrastructure.Persistence.Repositories.Base
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly VehicleCrmDbContext _context;

        public BaseRepository(VehicleCrmDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual async Task InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Set<T>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException($"Entity with id {id} not found");

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<T> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException($"Entity with id {id} not found");

            return entity;
        }
    }
}
