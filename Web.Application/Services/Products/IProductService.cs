using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs;

namespace Web.Application.Services.Products 
{
    public interface IProductService
    {
        public List<ProductDto> GetProductAlls();
        public ProductDto GetProductById(int id);
        public Task<int> CreateProductAsync(ProductDto product);
        public Task<ProductDto> UpdateProductAsync(ProductDto productDto);
        public Task DeleteProductAsync(int id);
    }
}
