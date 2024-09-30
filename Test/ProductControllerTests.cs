using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Api;
using Web.Api.Controllers;
using Web.Application.DTOs;
using Web.Application.Services.Products;
using Web.Domain.Entities;
using Web.Domain.Repositories;
using Xunit;
namespace Test
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductController _productController;

        public ProductControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _productController = new ProductController(_mockProductService.Object);
            var config = new MapperConfiguration(cfg =>
            {
                // Cấu hình AutoMapper của bạn ở đây
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetProductById_ShouldReturnOk_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var product = new ProductDto
            {
                Id = productId,
                Name = "Test Product",
                Price = 100
            };

            _mockProductService.Setup(service => service.GetProductById(productId)).Returns(product);


            // Act
            var result =  _productController.GetProductById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(productId, returnedProduct.Id);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;
            _mockProductService.Setup(service => service.GetProductById(productId))
                .Returns((ProductDto)null);

            // Act
            var result =  _productController.GetProductById(productId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }

}
