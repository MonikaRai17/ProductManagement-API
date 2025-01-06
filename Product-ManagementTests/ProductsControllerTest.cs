using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using Product_Management_API.Controllers;
using ProductManagementModel.Models;
using ProductManagementServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Product_ManagementTests
{
    public class ProductsControllerTests
    {
        private readonly ProductsController _productcontroller;
        private readonly Mock<IProductManagementService> _productManagementService;

        public ProductsControllerTests()
        {
            _productManagementService = new Mock<IProductManagementService>();
            _productcontroller = new ProductsController(_productManagementService.Object);


        }

        [Fact]
        public void GetProducts_ReturnsOkResult()
        {
            var products = new List<Product>
            {
                new Product{Id = 1, Name ="data1", Price=100, Quantity=1},
                new Product{Id = 2, Name ="data2", Price=200, Quantity=2},
            };

            _productManagementService.Setup(ser => ser.GetAllProducts())
                .ReturnsAsync(products);

            var result = _productcontroller.Get("", 1, 1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var products1 = Assert.IsType<List<Product>>(okResult.Value);
            Assert.NotNull(products1);
        }

       

        [Fact]
        public async void GetProductbyId_ReturnsOkResult()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product", Price = 10, Quantity = 10 };
            _productManagementService.Setup(s => s.GetProductById(1)).ReturnsAsync(product);

            // Act
            var result = await _productcontroller.Get(1);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(product, okResult.Value);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _productManagementService.Setup(s => s.GetProductById(1)).ReturnsAsync((Product)null);

            // Act
            var result = await _productcontroller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Product_Add_Data_Return_OkResult()
        {
            //Arrange
            var product = new Product()
            {
                Id = 1,
                Name = "Test data 3",
                Price = 200,
                Quantity = 2

            };
            //Act
            var data =  _productcontroller.Post(product);
            //Assert
           
            Assert.NotNull(data); // Check that the user was added and returned
            Assert.Equal(product.Id, data.Id); // Check that the ID is correct
        }

        [Fact]
        public async void Product_Update_Data_Return_OkResult()
        {
            //Arrange
            var product = new Product()
            {
                Id = 1,
                Name = "Test data 3",
                Price = 200,
                Quantity = 2

            };


            //Act
            await _productcontroller.Put(product.Id,product);
            var result = _productcontroller.Get(1);
            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var products1 = Assert.IsType<List<Product>>(okResult);
            Assert.NotNull(products1);


            
        }


     

        [Fact]
        public void Delete_DeleteProductCorrectly()
        {
            // Arrange
            var Id = 1;

            // Act
            
            var result = _productcontroller.Delete(Id);
            // Assert
            Assert.IsType<OkResult>(result.Result); // Check that the data was deleted and cannot be found
        }


    }
}

