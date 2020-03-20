using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using ExpressMapper.Extensions;
using log4net;
using Newtonsoft.Json;
using ProductAPI.Core;
using ProductAPI.Core.Exceptions;
using ProductAPI.Models;
using ProductAPI.Services.Interfaces;
using Swashbuckle.Swagger.Annotations;

namespace ProductAPI.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private const string GetProductRouteName = "Product.Get";
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [Route("")]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ProductViewModel[]))]
        public IHttpActionResult Get()
        {
            var products = _productService.GetProducts();

            return Ok(products.Map<IList<ProductDTO>, List<ProductViewModel>>());
        }

        [Route("{id}", Name = GetProductRouteName)]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ProductViewModel))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Product with such id doesn't exist")]
       // [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(BadRequestResponseModel), Description = "Invalid id, should be int")]
        public IHttpActionResult Get(int id)
        {
            var product = _productService.GetProduct(id);

            if (product == null)
            {
                Logger.Error($"Couldn't find product with '{id}' identifier");
                return NotFound();
            }

            return Ok(product.Map<ProductDTO, ProductViewModel>());
        }

       [Route("")]
       [HttpPost]
       [SwaggerResponse(HttpStatusCode.Created, Type = typeof(ProductViewModel))]
       [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Invalid id, should be 0")]
        public IHttpActionResult Post(ProductViewModel product)
        {
            if (product.ProductId != default(int))
            {
                Logger.Error($"There is attempt to add already existing product: {JsonConvert.SerializeObject(product)}");
                return BadRequest("To add new product ID should not be specified");
            }

            var productToCreate = product.Map<ProductViewModel, ProductDTO>();

            try
            {
                _productService.CreateProduct(productToCreate);
                Logger.Debug($"New product was created: {JsonConvert.SerializeObject(product)}");
            }
            catch (HttpListenerException e)
            {
                Logger.Error($"The attempt to add product was failed. Details: {JsonConvert.SerializeObject(product)}", e);
            }


            var location = Url.Route(GetProductRouteName, new { id = productToCreate.ProductId });
            return Created(location, productToCreate.Map<ProductDTO, ProductViewModel>());
        }

        [Route("{id}")]
        [HttpPut]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ProductViewModel))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Product with such ID doesn't exist")]
        public IHttpActionResult Put(int id, ProductViewModel product)
        {
            var productToUpdate = product.Map<ProductViewModel, ProductDTO>();
            productToUpdate.ProductId = id;
            try
            {
                _productService.UpdateProduct(productToUpdate);
            }
            catch (ItemDoesntExistException)
            {
                return NotFound();
            }

            return Ok(productToUpdate.Map<ProductDTO, ProductViewModel>());

        }

        [Route("{id}")]
        [HttpDelete]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Invalid id, should be > 0")]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Product with such ID doesn't exist")]
        public IHttpActionResult Delete(int id)
        {
            if (id == default(int))
            {
                Logger.Error($"There is attempt to delete product with invalid ID");
                return BadRequest("Product ID should be provided");
            }

            try
            {
                _productService.DeleteProduct(id);
            }
            catch (ItemDoesntExistException)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
