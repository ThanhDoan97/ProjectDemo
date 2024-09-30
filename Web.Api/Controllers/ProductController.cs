using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Application.DTOs;
using Web.Application.Services.Products;
namespace Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-all-product")]
        [Authorize(Policy = "AdminOnly")] // Only Admin can create an account
        public IActionResult GetAll()
        {
            var products = _productService.GetProductAlls();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return ErrorResponse(product, "Not Found");
            }
            return SuccessResponse(product);

        }
        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDto productDto)
        {
            var product = _productService.CreateProductAsync(productDto);
            if (product == null)
            {
                return ErrorResponse(product, "Not Found");
            }
            return SuccessResponse(product);
        }
        [HttpPut]
        public IActionResult UpdateProduct([FromBody] ProductDto productDto)
        {
            var product = _productService.UpdateProductAsync(productDto);
            if (product == null)
            {
                return ErrorResponse(product, "NotFound");
            }
            return SuccessResponse(product);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var res = _productService.DeleteProductAsync(id);
            if (res == null)
            {
                return ErrorResponse(new ProductDto(), "NotFound");
            }
            return SuccessResponse(res);
        }


    }
}
