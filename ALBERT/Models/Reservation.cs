namespace ALBERT.Models
{
    public class Reservation: AggregateRoot
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; }
        public DateTime ReservationTime { get; set; }
        public ReservationStatus Status { get; set; }
    }




}
