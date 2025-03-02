using ALBERT.Models;

namespace ALBERT.ViewModels
{
    public class ReservationDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int TableId { get; set; }
        public DateTime ReservationTime { get; set; }
        public ReservationStatus Status { get; set; }
    }


}
