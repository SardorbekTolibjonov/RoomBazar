using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBazar.Api.Models;
using RoomBazar.Service.DTOs.Products;
using RoomBazar.Service.Interfaces.Products;

namespace RoomBazar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(ProductForCreationDto product)
        {
            var response = new Response 
            { 
                StatusCode = 200,
                Message = "Success",
                Data = await this.productService.AddProductAsync(product)
            };
            return Ok(response);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.productService.GetAllProductsAsync()
            };

            return Ok(response);
        }

    }
}
