using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs;
using Web.Domain;
using Web.Domain.Entities;
using Web.Domain.UnitOfWork;

namespace Web.Application.Services.Products 
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IDistributedCache _cache;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache; 
        }
        public List<ProductDto> GetProductAlls()
        {
            var res = new List<ProductDto>();
            var getstrCaches = GetCache("getProducts");
            if(getstrCaches == null)
            {
                var products = _unitOfWork.ProductRepository.GetAlls().ToList();
                 res = _mapper.Map<List<ProductDto>>(products);
                if(res != null) SetCache("getProducts", res);
            }
            else
            {

                res = JsonConvert.DeserializeObject<List<ProductDto>>(getstrCaches);
            }
       
            return res;
        }
        public  ProductDto GetProductById(int id)
        {
            var product =  _unitOfWork.ProductRepository.GetByIdAsync(id);
            var res = _mapper.Map<ProductDto>(product);
            return res;
        }
        public async Task<int> CreateProductAsync(ProductDto producDto)
        {
            try
            {
                var product = _mapper.Map<Product>(producDto);
                await _unitOfWork.ProductRepository.Create(product);
                await _unitOfWork.CompleteAsync();
                return product.Id;
            }
            catch (Exception ex)
            {
                _logger.Error("CreateProductAsync error", ex.Message);
                throw;
            }
        }

        public async Task<ProductDto> UpdateProductAsync(ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                await _unitOfWork.ProductRepository.Update(product);
                await _unitOfWork.CompleteAsync();
                productDto = _mapper.Map<ProductDto>(product);
                return productDto;
            }
            catch (Exception ex)
            {
                _logger.Error("UpdateProductAsync error", ex.Message);
                throw;
            }
        }
        public async Task DeleteProductAsync(int id)
        {
            try
            {
                await _unitOfWork.ProductRepository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.Error("DeleteProductAsync error", ex.Message);
                throw;
            }
        }

        public void SetCache(string key , object value)
        {
            var json = JsonConvert.SerializeObject(value);

            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Thời gian hết hạn
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30)); // Thời gian hết hạn tuyệt đối

             _cache.SetStringAsync(key, json, options);
        }
        public string GetCache(string key)
        {
            var value =  _cache.GetString(key);
            if (value == null)
            {
                return null;
            }
            return value;

        }


    }
}
