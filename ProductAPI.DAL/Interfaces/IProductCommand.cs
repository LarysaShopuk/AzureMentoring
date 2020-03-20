using ProductAPI.Core;

namespace ProductAPI.DAL.Interfaces
{
    public interface IProductCommand : ICommand
    {
        ProductDTO Create(ProductDTO product);

        void Update(ProductDTO product);

        void Delete(int productId);
    }
}
