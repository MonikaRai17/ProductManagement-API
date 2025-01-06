using Microsoft.AspNetCore.Mvc;
using ProductManagementServices.Services;
using System.Net;
using ProductManagementModel.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace Product_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductManagementService _productService;

        public ProductsController(IProductManagementService productService)
        {
            this._productService = productService; 
        }


        [HttpGet]
        public async Task<IActionResult> Get(string? searchItem, int _page, int _limit)
        {
            
                // List<Product> filterdata = null;
                var response = await this._productService.GetAllProducts();
               
                if (response != null)
                {
                    if (!String.IsNullOrEmpty(searchItem))
                    {
                        var filteredProducts = response
                        .Where(p => p.Name.Contains(searchItem, StringComparison.OrdinalIgnoreCase))
                        .Skip((_page - 1) * _limit)
                        .Take(_limit)
                        .ToList();

                        return Ok(filteredProducts);
                    }
                    else
                    {
                        var filteredProducts = response
                         .Skip((_page - 1) * _limit)
                         .Take(_limit)
                         .ToList();

                        return Ok(filteredProducts);
                    }
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, "Issue in API!");
                }
                throw new Exception("Exception while fetching all data.");

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
            throw new Exception("Exception while get the data.");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
           
                if (product == null) { return BadRequest(); }

                await _productService.AddProduct(product);
                return Ok();

                throw new Exception("Exception while add the data.");

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest();

            await _productService.UpdateProduct(product);
            return Ok();
            throw new Exception("Exception while update the data.");
    }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return BadRequest();
            await _productService.DeleteProduct(id);
            return Ok();
            throw new Exception("Exception while delete the data.");

        }
    }
}
