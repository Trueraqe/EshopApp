using EshopCore.Data;
using EshopCore.Database;
using EshopCore.Models;
using EshopCore.Services;
using EshopCore.Utils;
using EshopWebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;
using static EshopWebAPI.Utils.WebUtils;
using EshopCore.InterfacesWeb;
using EshopCore.Enums;
using EshopWebAPI.ModelsAPI;

namespace EshopWebAPI.Controllers
{
    public class OrderController : GetCurrentUserId
    {
        private readonly IOrderServiceWeb _orderService;
        public OrderController(IOrderServiceWeb orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> WebCreateOrder([FromForm] int userId)
        {
            var response = await _orderService.CreateOrder(userId);

            if (response == false)
                return NotFound();

            return Ok(response);
        }

        [HttpPost("LoggedInUserOdrersHistory")]
        public async Task<IActionResult> APILoggedInUserOdrersHistory([FromForm] int userId)
        {
            var response = await _orderService.LoggedInUserOdrersHistory(userId);

            return Ok(response);
        }

        [HttpPost("GetUserOrdersHistoryByUsername")]
        public async Task<IActionResult> APIGetUserOrdersHistoryByUsername([FromForm] string username)
        {
            var response = await _orderService.GetUserOrdersHistoryByUsername(username);

            return Ok(response);
        }

        [HttpGet("GetAllUsersOrders")]
        public async Task<IActionResult> APIGetAllUsersOrders()
        {
            var response = await _orderService.GetAllUsersOrders();

            return Ok(response);
        }

        [HttpPut("ChangeOrderStatus")]
        public async Task<IActionResult> APIChangeOrderStatus([FromBody] OrderStatusChangeAPI order)
        {
            var response = await _orderService.ChangeOrderStatus(order.Id, order.Status);

            if (response == false)
                return NotFound();

            return Ok(response);
        }
    }
}

