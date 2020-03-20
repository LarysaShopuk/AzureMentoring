using System;

namespace ProductAPI.DAL.Interfaces
{
    public interface IUnitOfWorkTransaction : IDisposable
    {
        void Commit();
    }
}
