using System;
using System.Collections.Generic;
using System.Data.Entity;
using ProductAPI.DAL.Interfaces;

namespace ProductAPI.DAL
{
    public class AdventureWorksUnityOfWork : IUnitOfWork
    {
        private readonly AdventureWorksContext _dbContext;

        public AdventureWorksUnityOfWork(AdventureWorksContext dbContext)
        {
            _dbContext = dbContext;
            Commands = new Dictionary<Type, Lazy<ICommand>>
            {
                {
                    typeof(IProductCommand), new Lazy<ICommand>(() => new ProductCommand(dbContext))
                }
            };
        }
        protected Dictionary<Type, Lazy<ICommand>> Commands { get; }

        public IUnitOfWorkTransaction BeginTransaction()
        {
            return new UnitOfWorkTransaction(_dbContext.Database.BeginTransaction());
        }

        public TCommand Get<TCommand>()
            where TCommand : ICommand
        {
            var type = typeof(TCommand);
            if (!Commands.ContainsKey(type))
            {
                throw new ArgumentException($"Command of type {type} not found");
            }

            return (TCommand)Commands[type].Value;
        }

        public void Commit()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch
            {
                ResetChanges();
                throw;
            }
        }

        public void ResetChanges()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
