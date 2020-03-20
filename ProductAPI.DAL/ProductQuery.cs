using System.Collections.Generic;
using System.Linq;
using ExpressMapper.Extensions;
using ProductAPI.Core;
using ProductAPI.DAL.Interfaces;

namespace ProductAPI.DAL
{
    public class ProductQuery : IProductQuery
    {
        private readonly AdventureWorksContext _context;

        public ProductQuery(AdventureWorksContext context)
        {
            _context = context;
        }

        public ProductDTO GetById(int productId)
        {
            var productEntity =_context.Products.FirstOrDefault(x=>x.ProductID == productId);

            return productEntity?.Map<Product, ProductDTO>();
        }

        public IList<ProductDTO> GetAll()
        {
            return _context.Products.ToList().Select(productEntity => productEntity.Map<Product, ProductDTO>()).ToList();
        }
    }
}
