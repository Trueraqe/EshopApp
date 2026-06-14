using EshopCore.Data;
using EshopCore.Database;
using EshopCore.InterfacesWeb;
using EshopCore.Models;
using EshopCore.Services;
using EshopCore.Utils;
using EshopWebAPI.ModelsAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;
using static EshopWebAPI.Utils.WebUtils;

namespace EshopWebAPI.Controllers
{
    public class CartController : GetCurrentUserId
    {
        private readonly ICartServiceWeb _cartService;
        public CartController(ICartServiceWeb cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("AddProductToCart")]
        public async Task<IActionResult> APIAddProductToCart([FromBody] CartAPI cart)
        {
            var response = await _cartService.AddProductToCart(cart.userId, cart.productId, cart.quantity);

            if (response == false)
                return NotFound();

            return Ok(response);
        }

        [HttpPost("RemoveProductFromCart")]
        public async Task<IActionResult> APIRemoveProductFromCart([FromBody] CartAPI cart)
        {
            var response = await _cartService.RemoveProductFromCart(cart.userId, cart.productId, cart.quantity);

            if (response == false)
                return NotFound();

            return Ok(response);
        }

        [HttpPost("ClearUserCart")]
        public IActionResult APIClearUserCart([FromForm] int userId)
        {
            return Ok(_cartService.ClearUserCart(userId));
        }

        [HttpPost("GetTotalCartPrice")]
        public async Task<IActionResult> APIGetTotalCartPrice([FromForm] int userId)
        {
            return Ok(await _cartService.GetTotalCartPrice(userId));
        }

        [HttpPost("GetUserCartItems")]
        public async Task<IActionResult> APIGetUserCartItems([FromForm] int userId)
        {
            return Ok(await _cartService.GetUserCartItems(userId));
        }
    }
}
