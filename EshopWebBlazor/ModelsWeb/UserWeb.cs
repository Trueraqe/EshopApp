using EshopShared.Enums;

namespace EshopWebBlazor.ModelsWeb
{
    public class UserWeb
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PasswordRepeat { get; set; } = null!;
        public UserRole Role { get; set; }
        public string CreatedAt {  get; set; }

        public List<OrderWeb> Orders { get; set; } = new();

        public UserWeb() { }
    }
}
