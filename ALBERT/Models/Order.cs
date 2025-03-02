namespace ALBERT.Models
{
    public class Order : AggregateRoot
    {
        public int TableId { get; set; }
        public Table Table { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public int WaiterId { get; set; }
        public Employee Waiter { get; set; }
    }




}
