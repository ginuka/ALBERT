namespace ALBERT.Models
{
    public class Payment: AggregateRoot
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        public string TransactionId { get; set; }
    }




}
