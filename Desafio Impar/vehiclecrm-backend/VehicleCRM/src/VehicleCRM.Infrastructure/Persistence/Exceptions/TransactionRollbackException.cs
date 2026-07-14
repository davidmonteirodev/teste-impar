namespace VehicleCRM.Infrastructure.Persistence.Exceptions
{
    public class TransactionRollbackException : Exception
    {
        public TransactionRollbackException()
            : base("Falha ao realizar o rollback da transação.")
        {
        }

        public TransactionRollbackException(string message)
            : base(message)
        {
        }

        public TransactionRollbackException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
