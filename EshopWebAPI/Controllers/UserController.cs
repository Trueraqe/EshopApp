using EshopCore.Data;
using EshopCore.InterfacesWeb;
using EshopCore.Models;
using EshopCore.Services;
using EshopCore.Utils;
using EshopWebAPI.ModelsAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using static EshopWebAPI.Utils.WebUtils;

namespace EshopWebAPI.Controllers
{
    public class UserController : GetCurrentUserId
    {
        private readonly IUserServiceWeb _userService;

        public UserController(IUserServiceWeb userService)
        {
            _userService = userService;
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> APIRegisterUser([FromBody] UserRegisterAPI user)
        {
            var register = await _userService.RegisterUser(user.Username, user.Email, user.Password);

            if (register == false)
                return NotFound();

            return Ok(register);
        }

        //[HttpPost("GetUserByUsername")]
        //public async Task<IActionResult> APIGetUserByUsername([FromForm] string username)
        //{
        //    var response = await _userService.GetUserByUsername(username);

        //    if (response == null)
        //        return NotFound();

        //    return Ok(response);
        //}

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> APIGetAllUsers()
        {
            var response = await _userService.GetAllUsers();

            return Ok(response);
        }
    }
}
