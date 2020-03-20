using System.Collections.Generic;
using ProductAPI.Core;

namespace ProductAPI.DAL.Interfaces
{
    public interface IProductQuery
    {
        ProductDTO GetById(int productId);

        IList<ProductDTO> GetAll();
    }
}
