namespace Do_an_lap_trinh_c_.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; } // price per item
        public string? Image { get; set; }
        public decimal Total => Price * Quantity;
    }
}
