using EshopShared.Enums;

namespace EshopWebBlazor.ModelsWeb
{
    public class OrderWeb
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public UserWeb Buyer { get; set; } = null!;

        public string CreatedAt { get; set; }
        public decimal? TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        public List<OrderItemWeb> Items { get; set; } = new();

        public OrderWeb() { }
    }
}
