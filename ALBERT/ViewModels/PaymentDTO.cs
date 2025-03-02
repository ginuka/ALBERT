using ALBERT.Models;

namespace ALBERT.ViewModels
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        public string TransactionId { get; set; }
    }

}
