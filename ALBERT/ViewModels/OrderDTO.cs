using ALBERT.Models;

namespace ALBERT.ViewModels
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public int WaiterId { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new();
    }


}
