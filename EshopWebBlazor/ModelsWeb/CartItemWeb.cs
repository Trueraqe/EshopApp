
namespace EshopWebBlazor.ModelsWeb
{
    public class CartItemWeb
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int ProductId { get; set; }
        public ProductWeb Product { get; set; } = null!;
        public int Quantity { get; set; }

        public CartItemWeb() { }
    }
}
