using EshopShared.Enums;

namespace EshopWebAPI.ModelsAPI
{
    public class UserLoginAPI
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
