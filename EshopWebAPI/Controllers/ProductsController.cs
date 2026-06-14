using EshopCore.Data;
using EshopCore.InterfacesWeb;
using EshopCore.Services;
using EshopCore.Utils;
using EshopWebAPI.ModelsAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using static EshopWebAPI.Utils.WebUtils;
using EshopCore.Models;

namespace EshopWebAPI.Controllers
{
    public class ProductsController : GetCurrentUserId
    {
        private readonly IProductServiceWeb _productsService;

        public ProductsController(IProductServiceWeb productsService)
        {
            _productsService = productsService;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("AddProduct")]
        public async Task<IActionResult> APIAddProduct([FromBody] Product product)
        {
            var response = await _productsService.AddProduct(product);

            if (response == false)
                return NotFound();

            return Ok(response);
        }

        [HttpPost("RemoveProduct")]
        public async Task<IActionResult> APIRemoveProduct([FromBody] Product product)
        {
            var response = await _productsService.RemoveProduct(product);

            if (response == false)
                return NotFound();

            return Ok(response);
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> APIUpdateProduct([FromBody] Product product)
        {
            var response = await _productsService.UpdateProduct(product);

            if (response == false)
                return NotFound();

            return Ok(response);
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> APIGetAllProducts()
        {
            var response = await _productsService.GetAllProducts();

            return Ok(response);
        }

        //[HttpPost("SearchProductByName")]
        //public async Task<IActionResult> APISearchProductByName([FromForm] string productName)
        //{
        //    var response = await _productsService.SearchProductByName(productName);

        //    if (response == null)
        //        return NotFound();

        //    return Ok(response);
        //}

        //[HttpPost("FilterProductsByCategory")]
        //public async Task<IActionResult> APIFilterProductsByCategory([FromForm] string productCategory)
        //{
        //    var response = await _productsService.FilterProductsByCategory(productCategory);

        //    if (response == null)
        //        return NotFound();

        //    return Ok(response);
        //}
    }
}
