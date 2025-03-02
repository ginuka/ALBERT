using ALBERT.Models;
using System.ComponentModel.DataAnnotations;

namespace ALBERT.ViewModels
{
    public class CreateReservationDto
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int TableId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ReservationTime { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;
    }
}
