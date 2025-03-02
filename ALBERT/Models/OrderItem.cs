namespace ALBERT.Models
{
    public class OrderItem: AggregateRoot
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }




}
