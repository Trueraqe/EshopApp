using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EshopWebAPI.Utils
{
    public class WebUtils
    {
        [ApiController]
        [Route("api/[controller]")]
        public abstract class GetCurrentUserId : ControllerBase
        {
            // Właściwość, która wyciąga ID i rzuca wyjątek, jeśli użytkownika nie ma
            protected int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("Użytkownik nie jest zalogowany"));
        }
        // var getUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // int userId = int.Parse(getUserId);

        // var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
