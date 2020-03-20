using System.Collections.Generic;
using ProductAPI.Core;

namespace ProductAPI.Services.Interfaces
{
    public interface IProductService
    {
        ProductDTO GetProduct(int id);

        IList<ProductDTO> GetProducts();

        void CreateProduct(ProductDTO product);

        void DeleteProduct(int productId);

        void UpdateProduct(ProductDTO product);
    }
}
