using System.Collections.Generic;
using ProductAPI.Core;
using ProductAPI.DAL.Interfaces;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductQuery _productQuery;
        private readonly IUnitOfWork _unitOfWork;

        

        public ProductService(IProductQuery productQuery, IUnitOfWork unitOfWork)
        {
            _productQuery = productQuery;
            _unitOfWork = unitOfWork;
        }

        public ProductDTO GetProduct(int id)
        {
            return _productQuery.GetById(id);
        }

        public IList<ProductDTO> GetProducts()
        {
            return _productQuery.GetAll();
        }

        public void CreateProduct(ProductDTO product)
        {
            var command = _unitOfWork.Get<IProductCommand>();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
               var createdProduct = command.Create(product);
                _unitOfWork.Commit();

                transaction.Commit();
                product.ProductId = createdProduct.ProductId;
            }
        }

        public void DeleteProduct(int productId)
        {
            var command = _unitOfWork.Get<IProductCommand>();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                command.Delete(productId);
                _unitOfWork.Commit();

                transaction.Commit();
            }
        }

        public void UpdateProduct(ProductDTO product)
        {
            var command = _unitOfWork.Get<IProductCommand>();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                command.Update(product);
                _unitOfWork.Commit();

                transaction.Commit();
            }
        }
    }
}
