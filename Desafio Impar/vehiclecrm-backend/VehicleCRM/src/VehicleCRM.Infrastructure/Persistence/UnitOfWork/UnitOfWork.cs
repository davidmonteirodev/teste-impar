using Microsoft.EntityFrameworkCore.Storage;
using VehicleCRM.Domain.Common.UnitOfWork;
using VehicleCRM.Infrastructure.Persistence.Contexts;
using VehicleCRM.Infrastructure.Persistence.Exceptions;

namespace VehicleCRM.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VehicleCrmDbContext _context;
        private IDbContextTransaction? _currentTransaction;
        private bool _disposed;
        private bool _committed;

        public UnitOfWork(VehicleCrmDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("Uma transação já está em andamento.");
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            _committed = false;
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("Nenhuma transação ativa para confirmar.");
            }

            try
            {
                await _currentTransaction.CommitAsync(cancellationToken);
                _committed = true;
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        private async Task RollbackTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                return;
            }

            try
            {
                await _currentTransaction.RollbackAsync();
            }
            catch (Exception ex)
            {
                throw new TransactionRollbackException("Falha ao realizar o rollback da transação.", ex);
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            if (_currentTransaction != null && !_committed)
            {
                try
                {
                    _currentTransaction.Rollback();
                }
                catch (Exception ex)
                {
                    throw new TransactionRollbackException("Falha ao realizar o rollback automático da transação no Dispose.", ex);
                }
            }

            _currentTransaction?.Dispose();
            _disposed = true;
        }
    }
}
