namespace Do_an_lap_trinh_c_.Models
{
    public class CartItem
    {
        public int Id { get; set; }           // ID sản phẩm
        public string Name { get; set; }      // Tên sản phẩm
        public string Image { get; set; }     // Đường dẫn ảnh
        public decimal Price { get; set; }    // Giá sản phẩm
        public int Quantity { get; set; }     // Số lượng
        public decimal Total => Price * Quantity;  // Tổng tiền
    }
}
