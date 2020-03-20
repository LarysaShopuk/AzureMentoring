using ProductAPI.DAL;
using ProductAPI.Services.Interfaces;
using Unity;

namespace ProductAPI.Services
{
    public class ServicesDependencyInitializer
    {
        public static void RegisterDependencies(IUnityContainer container)
        {
            DALDependencyInitializer.RegisterDependencies(container);
            container.RegisterType<IProductService, ProductService>();
        }
    }
}
