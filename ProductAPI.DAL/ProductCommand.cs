using System;
using System.Data.Entity;
using System.Linq;
using ExpressMapper.Extensions;
using ProductAPI.Core;
using ProductAPI.Core.Exceptions;
using ProductAPI.DAL.Interfaces;

namespace ProductAPI.DAL
{
    public class ProductCommand : IProductCommand
    {
        private readonly AdventureWorksContext _context;

        public ProductCommand(AdventureWorksContext context)
        {
            _context = context;
        }

        public ProductDTO Create(ProductDTO product)
        {
            var entity = product.Map<ProductDTO, Product>();
            entity.ModifiedDate = DateTime.UtcNow;

            var createdProduct = _context.Products.Add(entity);
            createdProduct.rowguid = Guid.NewGuid();

            return createdProduct.Map<Product, ProductDTO>();
        }


        public void Update(ProductDTO product)
        {
            if (_context.Products.FirstOrDefault(p => p.ProductID == product.ProductId) == null)
            {
                throw new ItemDoesntExistException($"Product with doesn't exist", product.ProductId);
            }

            var entity = product.Map(_context.Products.Local.FirstOrDefault(p => p.ProductID == product.ProductId));
            
            entity.ModifiedDate = DateTime.UtcNow;

            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int productId)
        {
            var entity = _context.Products.FirstOrDefault(p => p.ProductID == productId);
            if (entity == null)
            {
                throw new ItemDoesntExistException($"Product with doesn't exist", productId);
            }

            _context.Products.Remove(entity);
        }
    }
}
