using EshopCore.Enums;

namespace EshopWebAPI.ModelsAPI
{
    public class OrderStatusChangeAPI
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
    }
}
