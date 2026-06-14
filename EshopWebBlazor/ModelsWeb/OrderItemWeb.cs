
namespace EshopWebBlazor.ModelsWeb
{
    public class OrderItemWeb
    {
        public int? Id { get; set; }

        public int? OrderId { get; set; }
        public OrderWeb Order { get; set; } = null!;

        public int? ProductId { get; set; }
        public ProductWeb Product { get; set; } = null!;

        public int? Quantity { get; set; }
        public decimal? ItemTotalPrice { get; set; }

        public OrderItemWeb() { }
    }
}
