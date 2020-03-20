using System;

namespace ProductAPI.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        TCommand Get<TCommand>()
            where TCommand : ICommand;

        IUnitOfWorkTransaction BeginTransaction();

        void ResetChanges();

        void Commit();
    }
}
