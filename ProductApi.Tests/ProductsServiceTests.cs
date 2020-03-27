using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProductAPI.Core;
using ProductAPI.DAL.Interfaces;
using ProductAPI.Services;
using ProductAPI.Services.Interfaces;

namespace ProductApi.Tests
{
    [TestClass]
    public class ProductsServiceTests
    {
        private IProductService _productService;
        private Mock<IProductQuery> _productQueryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [TestInitialize]
        public void Init()
        {
            _productQueryMock = new Mock<IProductQuery>(MockBehavior.Strict);
            _unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            _productService = new ProductService(_productQueryMock.Object, _unitOfWorkMock.Object);
        }
        
        [TestMethod]
        [DynamicData(nameof(GetProduct_TestCases), DynamicDataSourceType.Method)]
        public void GetProducts_Should_return_products_that_returned_by_query(IList<ProductDTO> expectedProducts)
        {
            _productQueryMock.Setup(x => x.GetAll()).Returns(expectedProducts);

            var actualProducts = _productService.GetProducts();

            Assert.AreEqual(expectedProducts.Count, actualProducts.Count);
            foreach (var actualProduct in actualProducts)
            {
                Assert.IsTrue(expectedProducts.Any(x => x.ProductId == actualProduct.ProductId));
            }
        }

        private static IEnumerable<object[]> GetProduct_TestCases()
        {
            var product1 = new ProductDTO { Name = "Product1", ProductId = 1 };
            var product2 = new ProductDTO { Name = "Product2", ProductId = 2 };
            var product3 = new ProductDTO { Name = "Product3", ProductId = 3 };

            yield return new object[] { new List<ProductDTO>()};
            yield return new object[] { new List<ProductDTO> { product1 }};
            yield return new object[] { new List<ProductDTO> { product1, product2 }};
            yield return new object[] {new List<ProductDTO> { product1, product2, product3 }};
        }
    }
}
