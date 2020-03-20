using System.Data.Entity;
using ProductAPI.DAL.Interfaces;

namespace ProductAPI.DAL
{
    public class UnitOfWorkTransaction : IUnitOfWorkTransaction
    {
        private readonly DbContextTransaction _transaction;

        public UnitOfWorkTransaction(DbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public void Commit()
        {
            _transaction.Commit();
        }
    }
}
