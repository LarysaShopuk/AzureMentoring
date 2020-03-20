using System.Configuration;
using ProductAPI.DAL.Interfaces;
using Unity;

namespace ProductAPI.DAL
{
    public class DALDependencyInitializer
    {
        public static void RegisterDependencies(IUnityContainer container)
        {
            container.RegisterInstance<AdventureWorksContext>(new AdventureWorksContext(ConfigurationManager.ConnectionStrings["AdventureWorksEntities"].ConnectionString));
            container.RegisterType<IUnitOfWork, AdventureWorksUnityOfWork>();
            container.RegisterType<IUnitOfWorkTransaction, UnitOfWorkTransaction>();
            container.RegisterType<IProductQuery, ProductQuery>();
            container.RegisterType<IProductCommand, ProductCommand>();
        }
    }
}
