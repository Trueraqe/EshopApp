
namespace EshopWebBlazor.ModelsWeb
{
    public class ProductWeb
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; } = null!;

        public List<OrderItemWeb> OrderItems { get; set; } = new();

        public ProductWeb() { }
    }
}
