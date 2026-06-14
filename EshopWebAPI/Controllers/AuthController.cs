using EshopCore.Data;
using EshopCore.Database;
using EshopCore.InterfacesWeb;
using EshopCore.Models;
using EshopCore.Services;
using EshopCore.Utils;
using EshopWebAPI.Helpers;
using EshopWebAPI.ModelsAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using static EshopWebAPI.Utils.WebUtils;

namespace EshopWebAPI.Controllers
{
    public class AuthController : GetCurrentUserId
    {
        private readonly IAuthServiceWeb _authService;
        public AuthController(IAuthServiceWeb authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> APILogin([FromBody] UserLoginAPI user)
        {
            var auth = await _authService.Login(user.Username, user.Password);

            if (auth == null)
                return Unauthorized();

            return Ok(auth);
        }

        //[HttpGet("LoginTest")]
        //public IActionResult Test()
        //{
        //    return Ok(User.Identity?.IsAuthenticated);
        //    //return Ok(CurrentUserId);
        //}
    }
}

